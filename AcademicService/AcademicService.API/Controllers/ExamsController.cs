using AcademicService.BLL.Services.Interfaces;
using AcademicService.DAL.Data.MetaDatas;
using AcademicService.DAL.Data.Requests;
using Microsoft.AspNetCore.Mvc;

namespace AcademicService.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ExamsController : ControllerBase
{
    private readonly IExamService _examService;
    private readonly ILogger<ExamsController> _logger;

    public ExamsController(IExamService examService, ILogger<ExamsController> logger)
    {
        _examService = examService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllExams()
    {
        var exams = await _examService.GetAllExamsAsync();
        return Ok(ApiResponseBuilder.BuildResponse(200, "Exams retrieved successfully", exams));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetExamById(Guid id)
    {
        var exam = await _examService.GetExamByIdAsync(id);
        return Ok(ApiResponseBuilder.BuildResponse(200, "Exam retrieved successfully", exam));
    }

    [HttpPost]
    public async Task<IActionResult> CreateExam([FromBody] CreateExamRequest request)
    {
        var exam = await _examService.CreateExamAsync(request);
        return CreatedAtAction(nameof(GetExamById), new { id = exam.ExamId }, 
            ApiResponseBuilder.BuildResponse(201, "Exam created successfully", exam));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateExam(Guid id, [FromBody] UpdateExamRequest request)
    {
        var exam = await _examService.UpdateExamAsync(id, request);
        return Ok(ApiResponseBuilder.BuildResponse(200, "Exam updated successfully", exam));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteExam(Guid id)
    {
        await _examService.DeleteExamAsync(id);
        return Ok(ApiResponseBuilder.BuildResponse<object>(200, "Exam deleted successfully", null!));
    }
}
