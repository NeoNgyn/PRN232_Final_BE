using EzyFix.API.Constants;
using EzyFix.BLL.Services.Interfaces;
using EzyFix.DAL.Data.MetaDatas;
using EzyFix.DAL.Data.Requests.GradingDetails;
using EzyFix.DAL.Data.Responses.GradingDetails;
using Microsoft.AspNetCore.Mvc;

namespace EzyFix.API.Controllers
{
    [ApiController]
    public class GradingDetailsController : BaseController<GradingDetailsController>
    {
        private readonly IGradingDetailService _gradingDetailService;

        public GradingDetailsController(ILogger<GradingDetailsController> logger, IGradingDetailService gradingDetailService)
            : base(logger)
        {
            _gradingDetailService = gradingDetailService;
        }

        // ===============================
        // 1?? L?y danh sách chi ti?t ch?m ?i?m
        // ===============================
        [HttpGet(ApiEndPointConstant.GradingDetails.GradingDetailsEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<GradingDetailResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllGradingDetails()
        {
            var gradingDetails = await _gradingDetailService.GetAllGradingDetailsAsync();
            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK,
                "Grading detail list is retrieved successfully!",
                gradingDetails
            ));
        }

        // ===============================
        // 2?? L?y chi ti?t ch?m ?i?m theo ID
        // ===============================
        [HttpGet(ApiEndPointConstant.GradingDetails.GradingDetailEndpointById)]
        [ProducesResponseType(typeof(ApiResponse<GradingDetailResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetGradingDetailById(Guid id)
        {
            var gradingDetail = await _gradingDetailService.GetGradingDetailByIdAsync(id);
            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK,
                "Grading detail is retrieved successfully!",
                gradingDetail
            ));
        }

        // ===============================
        // 3?? L?y chi ti?t ch?m ?i?m theo ScoreId
        // ===============================
        [HttpGet(ApiEndPointConstant.GradingDetails.GradingDetailsByScoreEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<GradingDetailResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetGradingDetailsByScoreId(Guid scoreId)
        {
            var gradingDetails = await _gradingDetailService.GetGradingDetailsByScoreIdAsync(scoreId);
            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK,
                "Grading details by score ID retrieved successfully!",
                gradingDetails
            ));
        }

        // ===============================
        // 4?? L?y chi ti?t ch?m ?i?m theo ColumnId
        // ===============================
        [HttpGet(ApiEndPointConstant.GradingDetails.GradingDetailsByColumnEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<GradingDetailResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetGradingDetailsByColumnId(Guid columnId)
        {
            var gradingDetails = await _gradingDetailService.GetGradingDetailsByColumnIdAsync(columnId);
            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK,
                "Grading details by column ID retrieved successfully!",
                gradingDetails
            ));
        }

        // ===============================
        // 5?? T?o chi ti?t ch?m ?i?m m?i
        // ===============================
        [HttpPost(ApiEndPointConstant.GradingDetails.GradingDetailsEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<GradingDetailResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateGradingDetail([FromForm] CreateGradingDetailRequest request)
        {
            var response = await _gradingDetailService.CreateGradingDetailAsync(request);

            if (response == null)
            {
                return BadRequest(ApiResponseBuilder.BuildErrorResponse<object>(
                    null,
                    StatusCodes.Status400BadRequest,
                    "Cannot create new grading detail!",
                    "Grading detail creation failed!"
                ));
            }

            return CreatedAtAction(
                nameof(GetGradingDetailById),
                new { id = response.DetailId },
                ApiResponseBuilder.BuildResponse(
                    StatusCodes.Status201Created,
                    "Grading detail is created successfully!",
                    response
                )
            );
        }

        // ===============================
        // 6?? C?p nh?t chi ti?t ch?m ?i?m
        // ===============================
        [HttpPut(ApiEndPointConstant.GradingDetails.UpdateGradingDetailEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<GradingDetailResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateGradingDetail(Guid id, [FromForm] UpdateGradingDetailRequest request)
        {
            var updated = await _gradingDetailService.UpdateGradingDetailAsync(id, request);
            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK,
                "Grading detail is updated successfully!",
                updated
            ));
        }

        // ===============================
        // 7?? Xóa chi ti?t ch?m ?i?m
        // ===============================
        [HttpDelete(ApiEndPointConstant.GradingDetails.DeleteGradingDetailEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteGradingDetail(Guid id)
        {
            await _gradingDetailService.DeleteGradingDetailAsync(id);
            return Ok(ApiResponseBuilder.BuildResponse<object>(
                StatusCodes.Status200OK,
                "Grading detail is deleted successfully!",
                null
            ));
        }
    }
}