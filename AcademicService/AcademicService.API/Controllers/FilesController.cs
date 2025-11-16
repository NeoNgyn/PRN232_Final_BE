using AcademicService.BLL.Services.Implements;
using AcademicService.BLL.Services.Interfaces;
using AcademicService.DAL.Data.MetaDatas;
using AcademicService.DAL.Data.Requests.File;
using AcademicService.DAL.Data.Requests.Submission;
using ClosedXML.Excel;
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
        private readonly IExamService _examService;
        private readonly ILogger<FilesController> _logger;

        public FilesController(IFileService fileService, IStudentService studentService, ILogger<FilesController> logger, ICloudinaryService cloudinaryService, ISubmissionService submissionService, IExamService examService)
        {
            _fileService = fileService;
            _studentService = studentService;
            _logger = logger;
            _cloudinaryService = cloudinaryService;
            _submissionService = submissionService;
            _examService = examService;
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

            var fileUrl = await _cloudinaryService.UploadFileAsync(file, "FPT/students");

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


            // === CODE CŨ CỦA BẠN ===

            // === CODE CŨ CỦA BẠN ===
            var submissions = new List<object>();
            var errors = new List<string>();
            var tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            
            try
            {
                Directory.CreateDirectory(tempPath);
                var rarFilePath = Path.Combine(tempPath, extractRARRequest.RARFile.FileName);

                // Save archive file temporarily
                using (var stream = new FileStream(rarFilePath, FileMode.Create))
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
                            ExaminerId = metadata.ExaminerId
                        };
                            ExaminerId = metadata.ExaminerId
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

        [HttpGet("export-summary/{examId}")]
        public async Task<IActionResult> ExportSummaryExcel(
            Guid examId,
            [FromServices] ISubmissionService submissionService,
            [FromServices] ICritieriaService criteriaService)
        {
            // 1. Lấy toàn bộ submissions của exam
            var submissions = await submissionService.GetSubmissionsByExamIdAsync(examId);

            if (!submissions.Any())
            {
                return BadRequest(ApiResponseBuilder.BuildErrorResponse<object>(
                     null, 400, "Export failed", "No submissions in this exam"));
            }

            // 2. Kiểm tra bài nào chưa approve
            var unapproved = submissions.Where(x => x.IsApproved == false).ToList();

            if (unapproved.Any())
            {
                return BadRequest(ApiResponseBuilder.BuildErrorResponse<object>(
                   new
                   {
                       unapprovedCount = unapproved.Count,
                       unapprovedList = unapproved.Select(x => new
                       {
                           x.SubmissionId,
                           x.StudentId,
                           x.OriginalFileName
                       })
                   },
                   400,
                   "Export failed",
                   "There are unapproved submissions. Please approve all before exporting."
                ));
            }

            // 3. Lấy exam để tạo folder path
            var exam = await _examService.GetExamByIdAsync(examId);
            var folder = $"FPT/summary/{exam.Semester.SemesterCode}/{exam.Subject.SubjectCode}/{exam.ExamName}";

            var resultFiles = new List<object>();

            // 4. Lấy danh sách criteria
            var criteriaList = await criteriaService.GetCriteriasByExamIdAsync(examId);

            foreach (var sub in submissions)
            {
                using (var workbook = new XLWorkbook())
                {
                    var ws = workbook.Worksheets.Add("Grades");

                    // Header
                    ws.Cell("A1").Value = "Order";
                    ws.Cell("B1").Value = "Criteria";
                    ws.Cell("C1").Value = "Max Score";
                    ws.Cell("D1").Value = "Student Score";

                    int row = 2;

                    decimal total = 0;

                    foreach (var c in criteriaList.OrderBy(x => x.SortOrder))
                    {
                        var grade = sub.Grades.FirstOrDefault(g => g.CriteriaId == c.CriteriaId);

                        ws.Cell(row, 1).Value = c.SortOrder;
                        ws.Cell(row, 2).Value = c.CriteriaName;
                        ws.Cell(row, 3).Value = c.MaxScore;
                        ws.Cell(row, 4).Value = grade?.Score ?? 0;

                        total += grade?.Score ?? 0;
                        row++;
                    }

                    // Total row
                    ws.Cell(row, 3).Value = "TOTAL";
                    ws.Cell(row, 4).Value = total;

                    // Save to temp file
                    var tempPath = Path.Combine(Path.GetTempPath(), $"{sub.StudentId}_Student.xlsx");
                    workbook.SaveAs(tempPath);

                    // Upload lên cloudinary
                    using var stream = new FileStream(tempPath, FileMode.Open);
                    var formFile = new FormFile(stream, 0, stream.Length, null!, $"{sub.StudentId}_Student.xlsx");

                    var url = await _cloudinaryService.UploadFileAsync(formFile, folder);

                    resultFiles.Add(new
                    {
                        SubmissionId = sub.SubmissionId,
                        StudentId = sub.StudentId,
                        Url = url
                    });

                    System.IO.File.Delete(tempPath);
                }
            }

            return Ok(ApiResponseBuilder.BuildResponse(
                200,
                "Export successful",
                new
                {
                    examId,
                    exported = resultFiles.Count,
                    files = resultFiles
                }
            ));
        }

    }
}
