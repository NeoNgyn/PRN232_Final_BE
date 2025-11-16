using AcademicService.BLL.Services.Implements;
using AcademicService.BLL.Services.Interfaces;
using AcademicService.DAL.Data.MetaDatas;
using AcademicService.DAL.Data.Requests.File;
using AcademicService.DAL.Data.Requests.Submission;
using Microsoft.AspNetCore.Mvc;
using SharpCompress.Archives;

namespace AcademicService.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly IStudentService _studentService;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly ISubmissionService _submissionService;
        private readonly ILogger<FilesController> _logger;

        public FilesController(IFileService fileService, IStudentService studentService, ILogger<FilesController> logger, ICloudinaryService cloudinaryService, ISubmissionService submissionService)
        {
            _fileService = fileService;
            _studentService = studentService;
            _logger = logger;
            _cloudinaryService = cloudinaryService;
            _submissionService = submissionService;
        }

        [HttpGet("{filePath}")]
        public IActionResult GetFileUrl(string filePath)
        {
            var url = _fileService.GetFileUrl(filePath);
            return Ok(new { url });
        }

        [HttpPost("import-student")]
        public async Task<IActionResult> ImportStudents(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(ApiResponseBuilder.BuildErrorResponse<object>(
                    null,
                    StatusCodes.Status400BadRequest,
                    "Import failed",
                    "File is empty"
                ));
            }

            if (!file.FileName.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(ApiResponseBuilder.BuildErrorResponse<object>(
                    null,
                    StatusCodes.Status400BadRequest,
                    "Import failed",
                    "File must be a JSON file"
                ));
            }

            var fileUrl = await _cloudinaryService.UploadFileAsync(file, "academic/students");

            var result = await _studentService.ImportStudentsFromFileAsync(fileUrl, _fileService);

            var responseData = new
            {
                message = "Imported successfully",
                source = fileUrl,
                importedCount = result.Count(),
                students = result
            };

            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK,
                "Students imported successfully",
                responseData
            ));
        }

        [HttpPost("extract-rar")]
        public async Task<IActionResult> ExtractRarAndCreateSubmissions([FromForm] ExtractRARRequest extractRARRequest)
        {
            if (extractRARRequest.RARFile == null || extractRARRequest.RARFile.Length == 0)
            {
                return BadRequest(ApiResponseBuilder.BuildErrorResponse<object>(
                    null,
                    StatusCodes.Status400BadRequest,
                    "Extraction failed",
                    "RAR file is empty"
                ));
            }

            var archiveFileName = extractRARRequest.RARFile.FileName.ToLowerInvariant();
            if (!archiveFileName.EndsWith(".rar") && !archiveFileName.EndsWith(".zip"))
            {
                return BadRequest(ApiResponseBuilder.BuildErrorResponse<object>(
                    null,
                    StatusCodes.Status400BadRequest,
                    "Invalid file format",
                    "File must be a .rar or .zip archive"
                ));
            }

            var submissions = new List<object>();
            var errors = new List<string>();
            var tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

            try
            {
                Directory.CreateDirectory(tempPath);
                var rarFilePath = Path.Combine(tempPath, extractRARRequest.RARFile.FileName);

                // Save archive file temporarily
                using (var stream = new FileStream(rarFilePath, FileMode.Create))
                {
                    await extractRARRequest.RARFile.CopyToAsync(stream);
                }

                _logger.LogInformation($"Processing archive file: {extractRARRequest.RARFile.FileName}");

                // Extract files - ArchiveFactory supports both RAR and ZIP
                using (var archive = ArchiveFactory.Open(rarFilePath))
                {
                    var totalEntries = archive.Entries.Count(e => !e.IsDirectory);
                    _logger.LogInformation($"Found {totalEntries} files in archive");

                    foreach (var entry in archive.Entries.Where(e => !e.IsDirectory))
                    {
                        _logger.LogInformation($"Processing entry: {entry.Key}");

                        // Process DOC/DOCX files
                        if (entry.Key != null && (entry.Key.EndsWith(".doc", StringComparison.OrdinalIgnoreCase) ||
                            entry.Key.EndsWith(".docx", StringComparison.OrdinalIgnoreCase)))
                        {
                            try
                            {
                                _logger.LogInformation($"Processing document file: {entry.Key}");

                                var extractedFilePath = Path.Combine(tempPath, entry.Key);
                                Directory.CreateDirectory(Path.GetDirectoryName(extractedFilePath)!);
                                entry.WriteToFile(extractedFilePath);

                                _logger.LogInformation($"Extracted to: {extractedFilePath}");

                                // Parse StudentId from filename
                                var fileName = Path.GetFileNameWithoutExtension(entry.Key);
                                var studentId = ExtractStudentIdFromFileName(fileName);

                                if (string.IsNullOrEmpty(studentId))
                                {
                                    _logger.LogWarning($"Could not extract StudentId from filename: {entry.Key}");
                                    errors.Add($"Không thể trích xuất mã sinh viên từ file: {entry.Key}");
                                    continue;
                                }

                                _logger.LogInformation($"Extracted StudentId: {studentId} from file: {fileName}");

                                // Create FormFile for upload
                                await using var fileStream = new FileStream(extractedFilePath, FileMode.Open, FileAccess.Read);
                                var formFile = new FormFile(fileStream, 0, fileStream.Length, null!, Path.GetFileName(extractedFilePath))
                                {
                                    Headers = new HeaderDictionary(),
                                    ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
                                };

                                // Create submission
                                _logger.LogInformation($"Creating submission for ExamId: {extractRARRequest.ExamId}, ExaminerId: {extractRARRequest.ExaminerId}, StudentId: {studentId}");

                                var createRequest = new CreateSubmissionRequest
                                {
                                    ExamId = extractRARRequest.ExamId,
                                    ExaminerId = extractRARRequest.ExaminerId,
                                    StudentId = studentId
                                };

                                var created = await _submissionService.CreateSubmissionAsync(createRequest, formFile);
                                submissions.Add(created);

                                _logger.LogInformation($"Successfully created submission for student: {studentId}, SubmissionId: {created.SubmissionId}");
                            }
                            catch (Exception ex)
                            {
                                var innerException = ex.InnerException?.Message ?? "No inner exception";
                                var fullError = $"{ex.Message} | Inner: {innerException}";
                                _logger.LogError(ex, $"Error processing file: {entry.Key}. Details: {fullError}");
                                errors.Add($"Lỗi xử lý file {entry.Key}: {fullError}");
                            }
                        }
                        else
                        {
                            _logger.LogInformation($"Skipping non-document file: {entry.Key}");
                        }
                    }
                }

                // Clean up temp files
                Directory.Delete(tempPath, true);

                var response = new
                {
                    message = "Archive file extracted and processed",
                    totalFiles = submissions.Count + errors.Count,
                    successfulSubmissions = submissions.Count,
                    failedFiles = errors.Count,
                    submissions,
                    errors = errors.Any() ? errors : null
                };

                return Ok(ApiResponseBuilder.BuildResponse(
                    StatusCodes.Status200OK,
                    "Archive extracted and submissions created",
                    response
                ));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error extracting archive file");

                // Clean up on error
                if (Directory.Exists(tempPath))
                {
                    try { Directory.Delete(tempPath, true); } catch { }
                }

                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponseBuilder.BuildErrorResponse<object>(
                        null,
                        StatusCodes.Status500InternalServerError,
                        "Extraction failed",
                        ex.Message
                    ));
            }
        }

        // Helper method to extract StudentId from filename
        private string? ExtractStudentIdFromFileName(string fileName)
        {
            // Try multiple patterns:
            // Pattern 1: Any 2 letters + 6 digits anywhere in the name (e.g., SWD392_PE_SU25_SE184557_Name)
            var match = System.Text.RegularExpressions.Regex.Match(fileName, @"([A-Z]{2}\d{6})");
            if (match.Success)
            {
                _logger.LogInformation($"Matched StudentId: {match.Groups[1].Value} from filename: {fileName}");
                return match.Groups[1].Value;
            }

            // Pattern 2: Pure numbers 6-8 digits at the start (e.g., 123456_Name)
            match = System.Text.RegularExpressions.Regex.Match(fileName, @"^(\d{6,8})");
            if (match.Success)
            {
                _logger.LogInformation($"Matched StudentId: {match.Groups[1].Value} from filename: {fileName}");
                return match.Groups[1].Value;
            }

            _logger.LogWarning($"No StudentId pattern matched for filename: {fileName}");
            return null;
        }


        [HttpPost("import-criteria")]
        public async Task<IActionResult> ImportCriteria(
         IFormFile file,
        [FromForm] Guid examId,
        [FromServices] ICritieriaService criteriaService)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(ApiResponseBuilder.BuildErrorResponse<object>(
                    null,
                    StatusCodes.Status400BadRequest,
                    "Import failed",
                    "File is empty"
                ));
            }

            if (!file.FileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(ApiResponseBuilder.BuildErrorResponse<object>(
                    null,
                    StatusCodes.Status400BadRequest,
                    "Import failed",
                    "File must be an Excel (.xlsx)"
                ));
            }

            // Lưu file Excel tạm
            var tempFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + Path.GetExtension(file.FileName));
            await using (var stream = new FileStream(tempFilePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Đọc dữ liệu trong Excel
            var criterias = await _fileService.ReadCriteriasFromExcelAsync(tempFilePath, examId);

            if (!criterias.Any())
            {
                System.IO.File.Delete(tempFilePath);
                return BadRequest(ApiResponseBuilder.BuildErrorResponse<object>(
                    null,
                    StatusCodes.Status400BadRequest,
                    "Import failed",
                    "No valid criteria found in file"
                ));
            }

            var createdList = new List<object>();

            foreach (var criteria in criterias)
            {
                var created = await criteriaService.CreateCriteriaAsync(criteria);
                createdList.Add(created);
            }

            System.IO.File.Delete(tempFilePath);

            var responseData = new
            {
                message = "Imported successfully",
                importedCount = createdList.Count,
                criterias = createdList
            };

            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK,
                "Criteria imported successfully",
                responseData
            ));
        }
    }
}