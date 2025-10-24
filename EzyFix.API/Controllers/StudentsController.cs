using EzyFix.API.Constants;
using EzyFix.BLL.Services.Interfaces;
using EzyFix.DAL.Data.MetaDatas;
using EzyFix.DAL.Data.Requests.Students;
using EzyFix.DAL.Data.Responses.Students;
using Microsoft.AspNetCore.Mvc;

namespace EzyFix.API.Controllers
{
    [ApiController]
    public class StudentsController : BaseController<StudentsController>
    {
        private readonly IStudentService _studentService;

        public StudentsController(ILogger<StudentsController> logger, IStudentService studentService) : base(logger)
        {
            _studentService = studentService;
        }

        [HttpGet(ApiEndPointConstant.Students.StudentsEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<StudentResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await _studentService.GetAllStudentsAsync();
            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK, "Student list retrieved successfully", students
            ));
        }

        [HttpGet(ApiEndPointConstant.Students.StudentEndpointById)]
        [ProducesResponseType(typeof(ApiResponse<StudentResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetStudentById(Guid id)
        {
            var student = await _studentService.GetStudentByIdAsync(id);
            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK, "Student information retrieved successfully", student
            ));
        }

        [HttpPost(ApiEndPointConstant.Students.StudentsEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<StudentResponse>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateStudent([FromBody] CreateStudentRequest createDto)
        {
            var newStudent = await _studentService.CreateStudentAsync(createDto);
            return CreatedAtAction(
                nameof(GetStudentById),
                new { id = newStudent.StudentId },
                ApiResponseBuilder.BuildResponse(
                    StatusCodes.Status201Created, "Student created successfully", newStudent
                )
            );
        }

        [HttpPut(ApiEndPointConstant.Students.UpdateStudentEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<StudentResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateStudent(Guid id, [FromBody] UpdateStudentRequest updateDto)
        {
            var updatedStudent = await _studentService.UpdateStudentAsync(id, updateDto);
            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK, "Student updated successfully", updatedStudent
            ));
        }

        [HttpDelete(ApiEndPointConstant.Students.DeleteStudentEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteStudent(Guid id)
        {
            await _studentService.DeleteStudentAsync(id);
            return Ok(ApiResponseBuilder.BuildResponse<object>(
                StatusCodes.Status200OK, "Student deleted successfully", null
            ));
        }
    }
}