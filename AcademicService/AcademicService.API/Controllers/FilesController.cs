using AcademicService.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AcademicService.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class FilesController : ControllerBase
{
    private readonly IFileService _fileService;
    private readonly ILogger<FilesController> _logger;

    public FilesController(IFileService fileService, ILogger<FilesController> logger)
    {
        _fileService = fileService;
        _logger = logger;
    }

    [HttpGet("{filePath}")]
    public IActionResult GetFileUrl(string filePath)
    {
        var url = _fileService.GetFileUrl(filePath);
        return Ok(new { url });
    }
}
