using AcademicService.API.Constants;
using AcademicService.BLL.Services.Implements;
using AcademicService.BLL.Services.Interfaces;
using AcademicService.DAL.Data.MetaDatas;
using AcademicService.DAL.Data.Requests.Grade;
using AcademicService.DAL.Data.Requests.Violation;
using AcademicService.DAL.Data.Responses.Grade;
using AcademicService.DAL.Data.Responses.Violation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcademicService.API.Controllers
{
    [ApiController]
    public class ViolationController : BaseController<ViolationController>
    {
        private readonly IViolationService _violationService;
        public ViolationController(ILogger<ViolationController> logger, IViolationService violationService) : base(logger)
        {
            _violationService = violationService;
        }

        [HttpGet(ApiEndPointConstant.Violations.ViolationsEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ViolationListResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllViolations()
        {
            var userClaims = User.Claims.Select(c => new { c.Type, c.Value }); //debugging code
            _logger.LogInformation("User claims: {@Claims}", userClaims);

            var violations = await _violationService.GetAllViolationsAsync();
            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK,
                "Violation list retrieved successfully",
                violations
            ));
        }

        [HttpGet(ApiEndPointConstant.Violations.ViolationEndpointById)]
        [ProducesResponseType(typeof(ApiResponse<ViolationListResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetViolationById(Guid id)
        {
            var violation = await _violationService.GetViolationByIdAsync(id);
            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK,
                "Violation retrieved successfully",
                violation
            ));
        }

        [HttpPost(ApiEndPointConstant.Violations.ViolationsEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<ViolationListResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateViolation([FromBody] CreateViolationRequest request)
        {
            var response = await _violationService.CreateViolationAsync(request);

            if (response == null)
            {
                return BadRequest(
                    ApiResponseBuilder.BuildErrorResponse<object>(
                        null,
                        StatusCodes.Status400BadRequest,
                        "Failed to create violation",
                        "The violation creation process failed"
                    )
                );
            }

            return CreatedAtAction(
                nameof(GetViolationById),
                new { id = response.ViolationId },
                ApiResponseBuilder.BuildResponse(
                    StatusCodes.Status201Created,
                    "Violation created successfully",
                    response
                )
            );
        }

        [HttpPut(ApiEndPointConstant.Violations.UpdateViolationEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<ViolationListResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateViolation(Guid id, [FromForm] UpdateViolationRequest request)
        {
            var updatedViolation = await _violationService.UpdateViolationAsync(id, request);
            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK,
                "Violation updated successfully",
                updatedViolation
            ));
        }

        [HttpPatch(ApiEndPointConstant.Violations.DeleteViolationEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteViolaton(Guid id)
        {
            await _violationService.DeleteViolationAsync(id);
            return Ok(ApiResponseBuilder.BuildResponse<object>(
                StatusCodes.Status200OK,
                "Violation deleted successfully",
                null
            ));
        }
    }
}
