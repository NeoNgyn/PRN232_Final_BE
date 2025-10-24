using EzyFix.API.Constants;
using EzyFix.BLL.Services.Interfaces;
using EzyFix.DAL.Data.MetaDatas;
using EzyFix.DAL.Data.Requests.LecturerSubjects;
using EzyFix.DAL.Data.Responses.LecturerSubjects;
using Microsoft.AspNetCore.Mvc;

namespace EzyFix.API.Controllers
{
    [ApiController]
    public class LecturerSubjectsController : BaseController<LecturerSubjectsController>
    {
        private readonly ILecturerSubjectService _lecturerSubjectService;

        public LecturerSubjectsController(ILogger<LecturerSubjectsController> logger, ILecturerSubjectService lecturerSubjectService)
            : base(logger)
        {
            _lecturerSubjectService = lecturerSubjectService;
        }

        // ===============================
        // 1?? L?y danh sách phân công gi?ng viên
        // ===============================
        [HttpGet(ApiEndPointConstant.LecturerSubjects.LecturerSubjectsEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<LecturerSubjectResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllLecturerSubjects()
        {
            var lecturerSubjects = await _lecturerSubjectService.GetAllLecturerSubjectsAsync();
            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK,
                "Lecturer subject assignment list is retrieved successfully!",
                lecturerSubjects
            ));
        }

        // ===============================
        // 2?? L?y phân công gi?ng viên theo ID
        // ===============================
        [HttpGet(ApiEndPointConstant.LecturerSubjects.LecturerSubjectEndpointById)]
        [ProducesResponseType(typeof(ApiResponse<LecturerSubjectResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetLecturerSubjectById(Guid id)
        {
            var lecturerSubject = await _lecturerSubjectService.GetLecturerSubjectByIdAsync(id);
            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK,
                "Lecturer subject assignment is retrieved successfully!",
                lecturerSubject
            ));
        }

        // ===============================
        // 3?? L?y phân công theo gi?ng viên
        // ===============================
        [HttpGet(ApiEndPointConstant.LecturerSubjects.LecturerSubjectsByLecturerEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<LecturerSubjectResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetLecturerSubjectsByLecturerId(Guid lecturerId)
        {
            var lecturerSubjects = await _lecturerSubjectService.GetLecturerSubjectsByLecturerIdAsync(lecturerId);
            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK,
                "Lecturer subject assignments by lecturer ID retrieved successfully!",
                lecturerSubjects
            ));
        }

        // ===============================
        // 4?? L?y phân công theo môn h?c
        // ===============================
        [HttpGet(ApiEndPointConstant.LecturerSubjects.LecturerSubjectsBySubjectEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<LecturerSubjectResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetLecturerSubjectsBySubjectId(Guid subjectId)
        {
            var lecturerSubjects = await _lecturerSubjectService.GetLecturerSubjectsBySubjectIdAsync(subjectId);
            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK,
                "Lecturer subject assignments by subject ID retrieved successfully!",
                lecturerSubjects
            ));
        }

        // ===============================
        // 5?? L?y phân công theo h?c k?
        // ===============================
        [HttpGet(ApiEndPointConstant.LecturerSubjects.LecturerSubjectsBySemesterEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<LecturerSubjectResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetLecturerSubjectsBySemesterId(Guid semesterId)
        {
            var lecturerSubjects = await _lecturerSubjectService.GetLecturerSubjectsBySemesterIdAsync(semesterId);
            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK,
                "Lecturer subject assignments by semester ID retrieved successfully!",
                lecturerSubjects
            ));
        }

        // ===============================
        // 6?? T?o phân công gi?ng viên m?i
        // ===============================
        [HttpPost(ApiEndPointConstant.LecturerSubjects.LecturerSubjectsEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<LecturerSubjectResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateLecturerSubject([FromForm] CreateLecturerSubjectRequest request)
        {
            var response = await _lecturerSubjectService.CreateLecturerSubjectAsync(request);

            if (response == null)
            {
                return BadRequest(ApiResponseBuilder.BuildErrorResponse<object>(
                    null,
                    StatusCodes.Status400BadRequest,
                    "Cannot create new lecturer subject assignment!",
                    "Lecturer subject assignment creation failed!"
                ));
            }

            return CreatedAtAction(
                nameof(GetLecturerSubjectById),
                new { id = response.LecturerSubjectId },
                ApiResponseBuilder.BuildResponse(
                    StatusCodes.Status201Created,
                    "Lecturer subject assignment is created successfully!",
                    response
                )
            );
        }

        // ===============================
        // 7?? C?p nh?t phân công gi?ng viên
        // ===============================
        [HttpPut(ApiEndPointConstant.LecturerSubjects.UpdateLecturerSubjectEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<LecturerSubjectResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateLecturerSubject(Guid id, [FromForm] UpdateLecturerSubjectRequest request)
        {
            var updated = await _lecturerSubjectService.UpdateLecturerSubjectAsync(id, request);
            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK,
                "Lecturer subject assignment is updated successfully!",
                updated
            ));
        }

        // ===============================
        // 8?? Xóa phân công gi?ng viên
        // ===============================
        [HttpDelete(ApiEndPointConstant.LecturerSubjects.DeleteLecturerSubjectEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteLecturerSubject(Guid id)
        {
            await _lecturerSubjectService.DeleteLecturerSubjectAsync(id);
            return Ok(ApiResponseBuilder.BuildResponse<object>(
                StatusCodes.Status200OK,
                "Lecturer subject assignment is deleted successfully!",
                null
            ));
        }
    }
}