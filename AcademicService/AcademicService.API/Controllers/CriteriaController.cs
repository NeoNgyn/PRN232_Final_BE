using AcademicService.API.Constants;
using AcademicService.BLL.Services.Implements;
using AcademicService.BLL.Services.Interfaces;
using AcademicService.DAL.Data.MetaDatas;
using AcademicService.DAL.Data.Requests.Criteria;
using AcademicService.DAL.Data.Requests.Submission;
using AcademicService.DAL.Data.Responses.Criteria;
using AcademicService.DAL.Data.Responses.Submission;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace AcademicService.API.Controllers
{
    [ApiController]
    public class CriteriaController : BaseController<CriteriaController>
    {
        private readonly ICritieriaService _criteriaService;

        public CriteriaController(ILogger<CriteriaController> logger, ICritieriaService critieriaService) : base(logger)
        {
            _criteriaService = critieriaService;
        }

        [HttpGet(ApiEndPointConstant.Criterias.CriteriasEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<CriteriaListResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllCriterias([FromQuery] CriteriaQueryParameter queryParameter)
        {
            var userClaims = User.Claims.Select(c => new { c.Type, c.Value }); //debugging code
            _logger.LogInformation("User claims: {@Claims}", userClaims);

            var criterias = await _criteriaService.GetAllCriteriasAsync(queryParameter);
            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK,
                "Criteria list retrieved successfully",
                criterias
            ));
        }

        [HttpGet(ApiEndPointConstant.Criterias.QueryCriteriaEndpoint)]
        [EnableQuery]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<CriteriaListResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> QueryCriterias()
        {
            var userClaims = User.Claims.Select(c => new { c.Type, c.Value }); //debugging code
            _logger.LogInformation("User claims: {@Claims}", userClaims);

            var criterias = await _criteriaService.GetQueryCriteriasAsync();
            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK,
                "Criteria list retrieved successfully",
                criterias
            ));
        }

        [HttpGet(ApiEndPointConstant.Criterias.QueryCriteriaEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<CriteriaListResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCriteriaById(Guid id)
        {
            var criteria = await _criteriaService.GetCriteriaByIdAsync(id);
            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK,
                "Criteria retrieved successfully",
                criteria
            ));
        }

        [HttpPost(ApiEndPointConstant.Criterias.CriteriasEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<SubmissionListResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCriteria([FromBody] CreateCriteriaRequest request)
        {
            var response = await _criteriaService.CreateCriteriaAsync(request);

            if (response == null)
            {
                return BadRequest(
                    ApiResponseBuilder.BuildErrorResponse<object>(
                        null,
                        StatusCodes.Status400BadRequest,
                        "Failed to create criteria",
                        "The criteria creation process failed"
                    )
                );
            }

            return CreatedAtAction(
                nameof(GetCriteriaById),
                new { id = response.CriteriaId },
                ApiResponseBuilder.BuildResponse(
                    StatusCodes.Status201Created,
                    "Criteria created successfully",
                    response
                )
            );
        }

        [HttpPut(ApiEndPointConstant.Criterias.CriteriasEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<CriteriaListResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateSubmission(Guid id, [FromForm] UpdateCriteriaRequest request)
        {
            var updatedCriteria = await _criteriaService.UpdateCriteriaAsync(id, request);
            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK,
                "Criteria updated successfully",
                updatedCriteria
            ));
        }

        [HttpDelete(ApiEndPointConstant.Criterias.DeleteCriteriaEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAddress(Guid id)
        {
            await _criteriaService.DeleteCriteriaAsync(id);
            return Ok(ApiResponseBuilder.BuildResponse<object>(
                StatusCodes.Status200OK,
                "Criteria deleted successfully",
                null
            ));
        }

    }
}
