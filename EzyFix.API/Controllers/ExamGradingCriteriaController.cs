using EzyFix.API.Constants;
using EzyFix.BLL.Services.Interfaces;
using EzyFix.DAL.Data.MetaDatas;
using EzyFix.DAL.Data.Requests.ExamGradingCriteria;
using EzyFix.DAL.Data.Responses.ExamGradingCriteria;
using Microsoft.AspNetCore.Mvc;

namespace EzyFix.API.Controllers
{
    [ApiController]
    public class ExamGradingCriteriaController : BaseController<ExamGradingCriteriaController>
    {
        private readonly IExamGradingCriterionService _examGradingCriterionService;

        public ExamGradingCriteriaController(ILogger<ExamGradingCriteriaController> logger, IExamGradingCriterionService examGradingCriterionService) : base(logger)
        {
            _examGradingCriterionService = examGradingCriterionService;
        }

        [HttpGet(ApiEndPointConstant.ExamGradingCriteria.ExamGradingCriteriaEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ExamGradingCriterionResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllExamGradingCriteria()
        {
            var criteria = await _examGradingCriterionService.GetAllExamGradingCriteriaAsync();
            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK, "Exam grading criteria list retrieved successfully", criteria
            ));
        }

        [HttpGet(ApiEndPointConstant.ExamGradingCriteria.ExamGradingCriterionEndpointById)]
        [ProducesResponseType(typeof(ApiResponse<ExamGradingCriterionResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetExamGradingCriterionById(Guid id)
        {
            var criterion = await _examGradingCriterionService.GetExamGradingCriterionByIdAsync(id);
            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK, "Exam grading criterion retrieved successfully", criterion
            ));
        }

        [HttpPost(ApiEndPointConstant.ExamGradingCriteria.ExamGradingCriteriaEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<ExamGradingCriterionResponse>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateExamGradingCriterion([FromBody] CreateExamGradingCriterionRequest createDto)
        {
            var newCriterion = await _examGradingCriterionService.CreateExamGradingCriterionAsync(createDto);
            return CreatedAtAction(
                nameof(GetExamGradingCriterionById),
                new { id = newCriterion.CriteriaId },
                ApiResponseBuilder.BuildResponse(
                    StatusCodes.Status201Created, "Exam grading criterion created successfully", newCriterion
                )
            );
        }

        [HttpPut(ApiEndPointConstant.ExamGradingCriteria.UpdateExamGradingCriterionEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<ExamGradingCriterionResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateExamGradingCriterion(Guid id, [FromBody] UpdateExamGradingCriterionRequest updateDto)
        {
            var updatedCriterion = await _examGradingCriterionService.UpdateExamGradingCriterionAsync(id, updateDto);
            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK, "Exam grading criterion updated successfully", updatedCriterion
            ));
        }

        [HttpDelete(ApiEndPointConstant.ExamGradingCriteria.DeleteExamGradingCriterionEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteExamGradingCriterion(Guid id)
        {
            await _examGradingCriterionService.DeleteExamGradingCriterionAsync(id);
            return Ok(ApiResponseBuilder.BuildResponse<object>(
                StatusCodes.Status200OK, "Exam grading criterion deleted successfully", null
            ));
        }
    }
}