using AcademicService.BLL.Services.Interfaces;
using AcademicService.DAL.Data.MetaDatas;
using AcademicService.DAL.Data.Requests;
using Microsoft.AspNetCore.Mvc;

namespace AcademicService.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class SemestersController : ControllerBase
{
    private readonly ISemesterService _semesterService;
    private readonly ILogger<SemestersController> _logger;

    public SemestersController(ISemesterService semesterService, ILogger<SemestersController> logger)
    {
        _semesterService = semesterService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllSemesters()
    {
        var semesters = await _semesterService.GetAllSemestersAsync();
        return Ok(ApiResponseBuilder.BuildResponse(200, "Semesters retrieved successfully", semesters));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSemesterById(Guid id)
    {
        var semester = await _semesterService.GetSemesterByIdAsync(id);
        return Ok(ApiResponseBuilder.BuildResponse(200, "Semester retrieved successfully", semester));
    }

    [HttpPost]
    public async Task<IActionResult> CreateSemester([FromBody] CreateSemesterRequest request)
    {
        var semester = await _semesterService.CreateSemesterAsync(request);
        return CreatedAtAction(nameof(GetSemesterById), new { id = semester.SemesterId }, 
            ApiResponseBuilder.BuildResponse(201, "Semester created successfully", semester));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSemester(Guid id, [FromBody] UpdateSemesterRequest request)
    {
        var semester = await _semesterService.UpdateSemesterAsync(id, request);
        return Ok(ApiResponseBuilder.BuildResponse(200, "Semester updated successfully", semester));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSemester(Guid id)
    {
        await _semesterService.DeleteSemesterAsync(id);
        return Ok(ApiResponseBuilder.BuildResponse<object>(200, "Semester deleted successfully", null!));
    }
}
