
using EzyFix.API.Constants;
using EzyFix.BLL.Services.Interfaces;
using EzyFix.DAL.Data.MetaDatas;
using EzyFix.DAL.Data.Requests.Assignments;
using EzyFix.DAL.Data.Responses.Assignments;
using Microsoft.AspNetCore.Mvc;

namespace EzyFix.API.Controllers
{
    [ApiController]
    public class AssignmentsController : BaseController<AssignmentsController>
    {
        private readonly IAssignmentService _assignmentService;
        public AssignmentsController(ILogger<AssignmentsController> logger, IAssignmentService assignmentService)
            : base(logger)
        {
            _assignmentService = assignmentService;
        }

        // ===============================
        // 1️⃣ Lấy danh sách bài tập
        // ===============================
        [HttpGet(ApiEndPointConstant.Assignments.AssignmentsEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<AssignmentResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllAssignments()
        {
            var assignments = await _assignmentService.GetAllAssignmentsAsync();
            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK,
                "Assignment list is retrieved successfully!",
                assignments
            ));
        }

        // ===============================
        // 2️⃣ Lấy bài tập theo ID
        // ===============================
        [HttpGet(ApiEndPointConstant.Assignments.AssignmentEndpointById)]
        [ProducesResponseType(typeof(ApiResponse<AssignmentResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAssignmentById(Guid id)
        {
            var assignment = await _assignmentService.GetAssignmentByIdAsync(id);
            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK,
                "Assignment is retrieved successfully!",
                assignment
            ));
        }

        // ===============================
        // 3️⃣ Tạo bài tập mới
        // ===============================
        [HttpPost(ApiEndPointConstant.Assignments.AssignmentsEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<AssignmentResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAssignment([FromForm] CreateAssignmentRequest request)
        {
            var response = await _assignmentService.CreateAssignmentAsync(request);

            if (response == null)
            {
                return BadRequest(ApiResponseBuilder.BuildErrorResponse<object>(
                    null,
                    StatusCodes.Status400BadRequest,
                    "Cannot create new assignment!",
                    "Assignment creation failed!"
                ));
            }

            return CreatedAtAction(
                nameof(GetAssignmentById),
                new { id = response.AssignmentId },
                ApiResponseBuilder.BuildResponse(
                    StatusCodes.Status201Created,
                    "Assignment is created successfully!",
                    response
                )
            );
        }

        // ===============================
        // 4️⃣ Cập nhật bài tập
        // ===============================
        [HttpPut(ApiEndPointConstant.Assignments.UpdateAssignmentEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<AssignmentResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAssignment(Guid id, [FromForm] UpdateAssignmentRequest request)
        {
            var updated = await _assignmentService.UpdateAssignmentAsync(id, request);
            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK,
                "Assignment is updated successfully!",
                updated
            ));
        }

        // ===============================
        // 5️⃣ Xóa bài tập
        // ===============================
        [HttpDelete(ApiEndPointConstant.Assignments.DeleteAssignmentEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAssignment(Guid id)
        {
            await _assignmentService.DeleteAssignmentAsync(id);
            return Ok(ApiResponseBuilder.BuildResponse<object>(
                StatusCodes.Status200OK,
                "Assignment is deleted successfully!",
                null
            ));
        }

        // ===============================
        // 6️⃣ Lấy danh sách bài tập theo sinh viên
        // ===============================
        [HttpGet(ApiEndPointConstant.Assignments.AssignmentsByStudentEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<AssignmentResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAssignmentsByStudent(Guid studentId)
        {
            var assignments = await _assignmentService.GetAssignmentsByStudentAsync(studentId);
            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK,
                "Student assignments are retrieved successfully!",
                assignments
            ));
        }

        // ===============================
        // 7️⃣ Lấy danh sách bài tập theo bài kiểm tra
        // ===============================
        [HttpGet(ApiEndPointConstant.Assignments.AssignmentsByExamEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<AssignmentResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAssignmentsByExam(Guid examId)
        {
            var assignments = await _assignmentService.GetAssignmentsByExamAsync(examId);
            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK,
                "Exam assignments are retrieved successfully!",
                assignments
            ));
        }

    }
}
