using AcademicService.BLL.Services.Implements;
using AcademicService.BLL.Services.Interfaces;
using AcademicService.DAL.Data.MetaDatas;
using AcademicService.DAL.Data.Requests.File;
using AcademicService.DAL.Data.Requests.Submission;
using Microsoft.AspNetCore.Mvc;
using SharpCompress.Archives;

namespace AcademicService.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class FilesController : ControllerBase
{
    private readonly IFileService _fileService;
    private readonly IStudentService _studentService;
    private readonly ICloudinaryService _cloudinaryService;
    private readonly ISubmissionService _submissionService;
    private readonly ILogger<FilesController> _logger;

    public FilesController(IFileService fileService, IStudentService studentService,  ILogger<FilesController> logger, ICloudinaryService cloudinaryService, ISubmissionService submissionService)
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
    public async Task<IActionResult> ImportStudents([FromForm] IFormFile file)
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

        if (!extractRARRequest.RARFile.FileName.EndsWith(".rar", StringComparison.OrdinalIgnoreCase))
        {
            return BadRequest(ApiResponseBuilder.BuildErrorResponse<object>(
                null,
                StatusCodes.Status400BadRequest,
                "Extraction failed",
                "File must be a .rar archive"
            ));
        }

        var submissions = new List<object>();
        var tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempPath);

        var rarFilePath = Path.Combine(tempPath, extractRARRequest.RARFile.FileName);

        // Lưu file RAR tạm
        using (var stream = new FileStream(rarFilePath, FileMode.Create))
        {
            await extractRARRequest.RARFile.CopyToAsync(stream);
        }

        // Giải nén file RAR
        using (var archive = SharpCompress.Archives.Rar.RarArchive.Open(rarFilePath))
        {
            foreach (var entry in archive.Entries.Where(e => !e.IsDirectory))
            {
                // Giải nén từng file DOC/DOCX ra temp folder
                if (entry.Key.EndsWith(".doc", StringComparison.OrdinalIgnoreCase) ||
                    entry.Key.EndsWith(".docx", StringComparison.OrdinalIgnoreCase))
                {
                    var extractedFilePath = Path.Combine(tempPath, entry.Key);
                    Directory.CreateDirectory(Path.GetDirectoryName(extractedFilePath)!);
                    entry.WriteToFile(extractedFilePath);

                    // Tạo file stream giả để upload lên Cloudinary
                    await using var fileStream = new FileStream(extractedFilePath, FileMode.Open, FileAccess.Read);
                    var formFile = new FormFile(fileStream, 0, fileStream.Length, null!, Path.GetFileName(extractedFilePath));

                    // Gọi logic tạo submission
                    var createRequest = new CreateSubmissionRequest
                    {
                        ExamId = extractRARRequest.ExamId,
                        ExaminerId = extractRARRequest.ExaminerId
                    };

                    var created = await _submissionService.CreateSubmissionAsync(createRequest, formFile);
                    submissions.Add(created);
                }
            }
        }

        // Xóa file tạm
        Directory.Delete(tempPath, true);

        var response = new
        {
            message = "RAR file extracted successfully",
            totalSubmissions = submissions.Count,
            submissions
        };

        return Ok(ApiResponseBuilder.BuildResponse(
            StatusCodes.Status200OK,
            "RAR extracted and submissions created successfully",
            response
        ));
    }


}
