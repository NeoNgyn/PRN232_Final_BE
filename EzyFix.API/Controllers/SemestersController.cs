using EzyFix.API.Constants;
using EzyFix.BLL.Services.Implements;
using EzyFix.BLL.Services.Interfaces;
using EzyFix.DAL.Data.MetaDatas;
using EzyFix.DAL.Data.Requests.Semesters;
using EzyFix.DAL.Data.Responses.Semesters;
using Microsoft.AspNetCore.Mvc;

namespace EzyFix.API.Controllers
{
    [ApiController]
    public class SemestersController : BaseController<SemestersController>
    {
        private readonly ISemesterService _semesterService;

        public SemestersController(ILogger<SemestersController> logger, ISemesterService semesterService)
            : base(logger)
        {
            _semesterService = semesterService;
        }

        // ===============================
        // 1️⃣ Lấy danh sách học kỳ
        // ===============================
        [HttpGet(ApiEndPointConstant.Semesters.SemestersEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<SemesterResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllSemesters()
        {
            var semesters = await _semesterService.GetAllSemestersAsync();
            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK,
                "Danh sách học kỳ được lấy thành công",
                semesters
            ));
        }

        // ===============================
        // 2️⃣ Lấy học kỳ theo ID
        // ===============================
        [HttpGet(ApiEndPointConstant.Semesters.SemesterEndpointById)]
        [ProducesResponseType(typeof(ApiResponse<SemesterResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSemesterById(string id)
        {
            var semester = await _semesterService.GetSemesterByIdAsync(id);
            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK,
                "Lấy thông tin học kỳ thành công",
                semester
            ));
        }

        // ===============================
        // 3️⃣ Tạo học kỳ mới
        // ===============================
        [HttpPost(ApiEndPointConstant.Semesters.SemestersEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<SemesterResponseDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateSemester([FromForm] CreateSemesterRequestDto request)
        {
            var response = await _semesterService.CreateSemesterAsync(request);

            if (response == null)
            {
                return BadRequest(ApiResponseBuilder.BuildErrorResponse<object>(
                    null,
                    StatusCodes.Status400BadRequest,
                    "Không thể tạo học kỳ mới",
                    "Quá trình tạo học kỳ thất bại"
                ));
            }

            return CreatedAtAction(
                nameof(GetSemesterById),
                new { id = response.SemesterId },
                ApiResponseBuilder.BuildResponse(
                    StatusCodes.Status201Created,
                    "Tạo học kỳ thành công",
                    response
                )
            );
        }

        // ===============================
        // 4️⃣ Cập nhật học kỳ
        // ===============================
        [HttpPut(ApiEndPointConstant.Semesters.UpdateSemesterEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<SemesterResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateSemester(string id, [FromForm] UpdateSemesterRequestDto request)
        {
            var updated = await _semesterService.UpdateSemesterAsync(id, request);
            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK,
                "Cập nhật học kỳ thành công",
                updated
            ));
        }

        // ===============================
        // 5️⃣ Xóa học kỳ
        // ===============================
        [HttpPut(ApiEndPointConstant.Semesters.DeleteSemesterEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteSemester(string id)
        {
            await _semesterService.DeleteSemesterAsync(id);
            return Ok(ApiResponseBuilder.BuildResponse<object>(
                StatusCodes.Status200OK,
                "Xóa học kỳ thành công",
                null
            ));
        }
    }
}
