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
        public async Task<IActionResult> ExtractRarAndCreateSubmissions(
    [FromForm] ExtractRARRequest metadata,
     IFormFile rarFile)
        {
            if (rarFile == null || rarFile.Length == 0)
            {
                return BadRequest(ApiResponseBuilder.BuildErrorResponse<object>(
                    null, 400, "Extraction failed", "RAR file is empty"));
            }

            if (!rarFile.FileName.EndsWith(".rar", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(ApiResponseBuilder.BuildErrorResponse<object>(
                    null, 400, "Extraction failed", "File must be a .rar archive"));
            }


            // === CODE CŨ CỦA BẠN ===
            var submissions = new List<object>();
            var tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempPath);

            var rarFilePath = Path.Combine(tempPath, rarFile.FileName);

            using (var stream = new FileStream(rarFilePath, FileMode.Create))
            {
                await rarFile.CopyToAsync(stream);
            }

            using (var archive = SharpCompress.Archives.Rar.RarArchive.Open(rarFilePath))
            {
                foreach (var entry in archive.Entries.Where(e => !e.IsDirectory))
                {
                    if (entry.Key.EndsWith(".doc", StringComparison.OrdinalIgnoreCase)
                        || entry.Key.EndsWith(".docx", StringComparison.OrdinalIgnoreCase))
                    {
                        var extractedFilePath = Path.Combine(tempPath, entry.Key);
                        Directory.CreateDirectory(Path.GetDirectoryName(extractedFilePath)!);
                        entry.WriteToFile(extractedFilePath);

                        await using var fileStream = new FileStream(extractedFilePath, FileMode.Open);
                        var formFile = new FormFile(fileStream, 0, fileStream.Length, null!, Path.GetFileName(extractedFilePath));



                        var createRequest = new CreateSubmissionRequest
                        {
                            ExamId = metadata.ExamId,
                            ExaminerId = metadata.ExaminerId
                        };

                        var created = await _submissionService.CreateSubmissionAsync(createRequest, formFile);
                        submissions.Add(created);
                    }
                }
            }

            Directory.Delete(tempPath, true);

            return Ok(ApiResponseBuilder.BuildResponse(
                200,
                "RAR extracted and submissions created successfully",
                new
                {
                    message = "RAR file extracted successfully",
                    totalSubmissions = submissions.Count,
                    submissions
                }
            ));
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
