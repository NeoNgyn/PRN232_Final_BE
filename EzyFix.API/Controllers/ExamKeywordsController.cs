using EzyFix.API.Constants;
using EzyFix.BLL.Services.Interfaces;
using EzyFix.DAL.Data.MetaDatas;
using EzyFix.DAL.Data.Requests.ExamKeyword;
using EzyFix.DAL.Data.Responses.ExamKeyword;
using Microsoft.AspNetCore.Mvc;


namespace EzyFix.API.Controllers
{
    [ApiController]
    public class ExamKeywordsController : BaseController<ExamKeywordsController>
    {
        private readonly IExamKeywordService _examKeywordService;

        public ExamKeywordsController(ILogger<ExamKeywordsController> logger, IExamKeywordService examKeywordService)
            : base(logger)
        {
            _examKeywordService = examKeywordService;
        }

        [HttpGet(ApiEndPointConstant.ExamKeywords.ExamKeywordsEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ExamKeywordResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllExamKeywords()
        {
            var examKeywords = await _examKeywordService.GetAllExamKeywordsAsync();
            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK,
                "Exam keyword list retrieved successfully",
                examKeywords
            ));
        }

        [HttpGet(ApiEndPointConstant.ExamKeywords.ExamKeywordEndpointById)]
        [ProducesResponseType(typeof(ApiResponse<ExamKeywordResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetExamKeywordById(Guid id)
        {
            var examKeyword = await _examKeywordService.GetExamKeywordByIdAsync(id);
            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK,
                "Exam keyword retrieved successfully",
                examKeyword
            ));
        }

        [HttpGet(ApiEndPointConstant.ExamKeywords.ExamKeywordsByExamEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ExamKeywordResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetExamKeywordsByExamId(Guid examId)
        {
            var examKeywords = await _examKeywordService.GetExamKeywordsByExamIdAsync(examId);
            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK,
                "Exam keywords by exam retrieved successfully",
                examKeywords
            ));
        }

        [HttpGet(ApiEndPointConstant.ExamKeywords.ExamKeywordsByKeywordEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ExamKeywordResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetExamKeywordsByKeywordId(Guid keywordId)
        {
            var examKeywords = await _examKeywordService.GetExamKeywordsByKeywordIdAsync(keywordId);
            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK,
                "Exam keywords by keyword retrieved successfully",
                examKeywords
            ));
        }

        [HttpPost(ApiEndPointConstant.ExamKeywords.ExamKeywordsEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<ExamKeywordResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateExamKeyword([FromBody] CreateExamKeywordRequest request)
        {
            var response = await _examKeywordService.CreateExamKeywordAsync(request);

            if (response == null)
            {
                return BadRequest(ApiResponseBuilder.BuildErrorResponse<object>(
                    null,
                    StatusCodes.Status400BadRequest,
                    "Cannot create exam keyword",
                    "Exam keyword creation failed"
                ));
            }

            return CreatedAtAction(
                nameof(GetExamKeywordById),
                new { id = response.ExamKeywordId },
                ApiResponseBuilder.BuildResponse(
                    StatusCodes.Status201Created,
                    "Exam keyword created successfully",
                    response
                )
            );
        }

        [HttpPut(ApiEndPointConstant.ExamKeywords.UpdateExamKeywordEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<ExamKeywordResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateExamKeyword(Guid id, [FromBody] UpdateExamKeywordRequest request)
        {
            var updated = await _examKeywordService.UpdateExamKeywordAsync(id, request);
            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK,
                "Exam keyword updated successfully",
                updated
            ));
        }

        [HttpDelete(ApiEndPointConstant.ExamKeywords.DeleteExamKeywordEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteExamKeyword(Guid id)
        {
            await _examKeywordService.DeleteExamKeywordAsync(id);
            return Ok(ApiResponseBuilder.BuildResponse<object>(
                StatusCodes.Status200OK,
                "Exam keyword deleted successfully",
                null
            ));
        }

        [HttpDelete(ApiEndPointConstant.ExamKeywords.DeleteExamKeywordsByExamEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteExamKeywordsByExamId(Guid examId)
        {
            await _examKeywordService.DeleteExamKeywordsByExamIdAsync(examId);
            return Ok(ApiResponseBuilder.BuildResponse<object>(
                StatusCodes.Status200OK,
                "All exam keywords for the exam deleted successfully",
                null
            ));
        }
    }
}
