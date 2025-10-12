using EzyFix.API.Constants;
using EzyFix.BLL.Services.Interfaces;
using EzyFix.DAL.Data.MetaDatas;
using EzyFix.DAL.Data.Requests.Subjects;
using EzyFix.DAL.Data.Responses.Subjects;
using Microsoft.AspNetCore.Mvc;

namespace EzyFix.API.Controllers
{
    [ApiController]
    public class SubjectsController : BaseController<SubjectsController>
    {
        private readonly ISubjectService _subjectService;

        public SubjectsController(ILogger<SubjectsController> logger, ISubjectService subjectService)
            : base(logger)
        {
            _subjectService = subjectService;
        }

        // ===============================
        // 1️⃣ Lấy danh sách môn học
        // ===============================
        [HttpGet(ApiEndPointConstant.Subjects.SubjectsEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<SubjectResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllSubjects()
        {
            var subjects = await _subjectService.GetAllSubjectsAsync();
            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK,
                "Danh sách môn học được lấy thành công",
                subjects
            ));
        }

        // ===============================
        // 2️⃣ Lấy môn học theo ID
        // ===============================
        [HttpGet(ApiEndPointConstant.Subjects.SubjectEndpointById)]
        [ProducesResponseType(typeof(ApiResponse<SubjectResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSubjectById(string id)
        {
            var subject = await _subjectService.GetSubjectByIdAsync(id);
            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK,
                "Lấy thông tin môn học thành công",
                subject
            ));
        }

        // ===============================
        // 3️⃣ Tạo môn học mới
        // ===============================
        [HttpPost(ApiEndPointConstant.Subjects.SubjectsEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<SubjectResponseDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateSubject([FromForm] CreateSubjectRequestDto request)
        {
            var response = await _subjectService.CreateSubjectAsync(request);

            if (response == null)
            {
                return BadRequest(ApiResponseBuilder.BuildErrorResponse<object>(
                    null,
                    StatusCodes.Status400BadRequest,
                    "Không thể tạo môn học mới",
                    "Quá trình tạo môn học thất bại"
                ));
            }

            return CreatedAtAction(
                nameof(GetSubjectById),
                new { id = response.SubjectId },
                ApiResponseBuilder.BuildResponse(
                    StatusCodes.Status201Created,
                    "Tạo môn học thành công",
                    response
                )
            );
        }

        // ===============================
        // 4️⃣ Cập nhật môn học
        // ===============================
        [HttpPut(ApiEndPointConstant.Subjects.UpdateSubjectEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<SubjectResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateSubject(string id, [FromForm] UpdateSubjectRequestDto request)
        {
            var updated = await _subjectService.UpdateSubjectAsync(id, request);
            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK,
                "Cập nhật môn học thành công",
                updated
            ));
        }

        // ===============================
        // 5️⃣ Xóa môn học
        // ===============================
        [HttpPut(ApiEndPointConstant.Subjects.DeleteSubjectEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteSubject(string id)
        {
            await _subjectService.DeleteSubjectAsync(id);
            return Ok(ApiResponseBuilder.BuildResponse<object>(
                StatusCodes.Status200OK,
                "Xóa môn học thành công",
                null
            ));
        }
    }
}
