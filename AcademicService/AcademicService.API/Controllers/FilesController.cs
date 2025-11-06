using AcademicService.BLL.Services.Implements;
using AcademicService.BLL.Services.Interfaces;
using AcademicService.DAL.Data.MetaDatas;
using Microsoft.AspNetCore.Mvc;

namespace AcademicService.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class FilesController : ControllerBase
{
    private readonly IFileService _fileService;
    private readonly IStudentService _studentService;
    private readonly ICloudinaryService _cloudinaryService;
    private readonly ILogger<FilesController> _logger;

    public FilesController(IFileService fileService, IStudentService studentService,  ILogger<FilesController> logger, ICloudinaryService cloudinaryService )
    {
        _fileService = fileService;
        _studentService = studentService;
        _logger = logger;
        _cloudinaryService = cloudinaryService;
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

}
