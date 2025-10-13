using EzyFix.API.Constants;
using EzyFix.BLL.Services.Interfaces;
using EzyFix.DAL.Data.MetaDatas;
using EzyFix.DAL.Data.Requests.Keywords;
using EzyFix.DAL.Data.Responses.Keywords;
using Microsoft.AspNetCore.Mvc;

namespace EzyFix.API.Controllers
{
    [ApiController]
    public class KeywordsController : BaseController<KeywordsController>
    {
        private readonly IKeywordService _keywordService;

        public KeywordsController(ILogger<KeywordsController> logger, IKeywordService keywordService)
            : base(logger)
        {
            _keywordService = keywordService;
        }

        // ===============================
        // 1️⃣ Lấy danh sách từ khóa
        // ===============================
        [HttpGet(ApiEndPointConstant.Keywords.KeywordsEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<KeywordResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllKeywords()
        {
            var keywords = await _keywordService.GetAllKeywordsAsync();
            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK,
                "Keyword list is retrieved successfully!",
                keywords
            ));
        }

        // ===============================
        // 2️⃣ Lấy từ khóa theo ID
        // ===============================
        [HttpGet(ApiEndPointConstant.Keywords.KeywordEndpointById)]
        [ProducesResponseType(typeof(ApiResponse<KeywordResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetKeywordById(Guid id)
        {
            var keyword = await _keywordService.GetKeywordByIdAsync(id);
            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK,
                "Keyword is retrieved successfully!",
                keyword
            ));
        }

        // ===============================
        // 3️⃣ Tạo từ khóa mới
        // ===============================
        [HttpPost(ApiEndPointConstant.Keywords.KeywordsEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<KeywordResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateKeyword([FromForm] CreateKeywordRequest request)
        {
            var response = await _keywordService.CreateKeywordAsync(request);

            if (response == null)
            {
                return BadRequest(ApiResponseBuilder.BuildErrorResponse<object>(
                    null,
                    StatusCodes.Status400BadRequest,
                    "Cannot create new keyword!",
                    "Keyword creation failed!"
                ));
            }

            return CreatedAtAction(
                nameof(GetKeywordById),
                new { id = response.KeywordId },
                ApiResponseBuilder.BuildResponse(
                    StatusCodes.Status201Created,
                    "Keyword is created successfully!",
                    response
                )
            );
        }

        // ===============================
        // 4️⃣ Cập nhật từ khóa
        // ===============================
        [HttpPut(ApiEndPointConstant.Keywords.UpdateKeywordEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<KeywordResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateKeyword(Guid id, [FromForm] UpdateKeywordRequest request)
        {
            var updated = await _keywordService.UpdateKeywordAsync(id, request);
            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK,
                "Keyword is updated successfully!",
                updated
            ));
        }

        // ===============================
        // 5️⃣ Xóa từ khóa
        // ===============================
        [HttpDelete(ApiEndPointConstant.Keywords.DeleteKeywordEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteKeyword(Guid id)
        {
            await _keywordService.DeleteKeywordAsync(id);
            return Ok(ApiResponseBuilder.BuildResponse<object>(
                StatusCodes.Status200OK,
                "Keyword is deleted successfully!",
                null
            ));
        }
    }
}
