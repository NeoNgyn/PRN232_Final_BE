using AcademicService.BLL.Services.Interfaces;
using AcademicService.DAL.Data.MetaDatas;
using AcademicService.DAL.Data.Requests;
using Microsoft.AspNetCore.Mvc;

namespace AcademicService.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class SubjectsController : ControllerBase
{
    private readonly ISubjectService _subjectService;
    private readonly ILogger<SubjectsController> _logger;

    public SubjectsController(ISubjectService subjectService, ILogger<SubjectsController> logger)
    {
        _subjectService = subjectService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllSubjects()
    {
        var subjects = await _subjectService.GetAllSubjectsAsync();
        return Ok(ApiResponseBuilder.BuildResponse(200, "Subjects retrieved successfully", subjects));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSubjectById(Guid id)
    {
        var subject = await _subjectService.GetSubjectByIdAsync(id);
        return Ok(ApiResponseBuilder.BuildResponse(200, "Subject retrieved successfully", subject));
    }

    [HttpPost]
    public async Task<IActionResult> CreateSubject([FromBody] CreateSubjectRequest request)
    {
        var subject = await _subjectService.CreateSubjectAsync(request);
        return CreatedAtAction(nameof(GetSubjectById), new { id = subject.SubjectId }, 
            ApiResponseBuilder.BuildResponse(201, "Subject created successfully", subject));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSubject(Guid id, [FromBody] UpdateSubjectRequest request)
    {
        var subject = await _subjectService.UpdateSubjectAsync(id, request);
        return Ok(ApiResponseBuilder.BuildResponse(200, "Subject updated successfully", subject));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSubject(Guid id)
    {
        await _subjectService.DeleteSubjectAsync(id);
        return Ok(ApiResponseBuilder.BuildResponse<object>(200, "Subject deleted successfully", null!));
    }
}
