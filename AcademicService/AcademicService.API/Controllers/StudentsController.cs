using AcademicService.BLL.Services.Interfaces;
using AcademicService.DAL.Data.MetaDatas;
using AcademicService.DAL.Data.Requests;
using Microsoft.AspNetCore.Mvc;

namespace AcademicService.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;
    private readonly ILogger<StudentsController> _logger;

    public StudentsController(IStudentService studentService, ILogger<StudentsController> logger)
    {
        _studentService = studentService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllStudents()
    {
        var students = await _studentService.GetAllStudentsAsync();
        return Ok(ApiResponseBuilder.BuildResponse(200, "Students retrieved successfully", students));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetStudentById(Guid id)
    {
        var student = await _studentService.GetStudentByIdAsync(id);
        return Ok(ApiResponseBuilder.BuildResponse(200, "Student retrieved successfully", student));
    }

    [HttpPost]
    public async Task<IActionResult> CreateStudent([FromBody] CreateStudentRequest request)
    {
        var student = await _studentService.CreateStudentAsync(request);
        return CreatedAtAction(nameof(GetStudentById), new { id = student.StudentId }, 
            ApiResponseBuilder.BuildResponse(201, "Student created successfully", student));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStudent(Guid id, [FromBody] UpdateStudentRequest request)
    {
        var student = await _studentService.UpdateStudentAsync(id, request);
        return Ok(ApiResponseBuilder.BuildResponse(200, "Student updated successfully", student));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStudent(Guid id)
    {
        await _studentService.DeleteStudentAsync(id);
        return Ok(ApiResponseBuilder.BuildResponse<object>(200, "Student deleted successfully", null!));
    }
}
