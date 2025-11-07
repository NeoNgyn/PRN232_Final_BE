using AcademicService.API.Constants;
using AcademicService.BLL.Services.Implements;
using AcademicService.BLL.Services.Interfaces;
using AcademicService.DAL.Data.MetaDatas;
using AcademicService.DAL.Data.Requests.Criteria;
using AcademicService.DAL.Data.Requests.Grade;
using AcademicService.DAL.Data.Responses.Criteria;
using AcademicService.DAL.Data.Responses.Grade;
using AcademicService.DAL.Data.Responses.Submission;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcademicService.API.Controllers
{
    [ApiController]
    public class GradeController : BaseController<GradeController>
    {
        private readonly IGradeService _gradeService;
        public GradeController(ILogger<GradeController> logger, IGradeService gradeService) : base(logger)
        {
            _gradeService = gradeService;
        }

        [HttpGet(ApiEndPointConstant.Grades.GradesEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<GradeListResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllGrades()
        {
            var userClaims = User.Claims.Select(c => new { c.Type, c.Value }); //debugging code
            _logger.LogInformation("User claims: {@Claims}", userClaims);

            var grades = await _gradeService.GetAllGradesAsync();
            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK,
                "Grade list retrieved successfully",
                grades
            ));
        }

        [HttpGet(ApiEndPointConstant.Grades.GradeEndpointById)]
        [ProducesResponseType(typeof(ApiResponse<GradeListResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetGradeById(Guid id)
        {
            var grade = await _gradeService.GetGradeByIdAsync(id);
            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK,
                "Grade retrieved successfully",
                grade
            ));
        }

        [HttpPost(ApiEndPointConstant.Grades.GradesEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<GradeListResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateGrade([FromBody] CreateGradeRequest request)
        {
            var response = await _gradeService.CreateGradeAsync(request);

            if (response == null)
            {
                return BadRequest(
                    ApiResponseBuilder.BuildErrorResponse<object>(
                        null,
                        StatusCodes.Status400BadRequest,
                        "Failed to create grade",
                        "The grade creation process failed"
                    )
                );
            }

            return CreatedAtAction(
                nameof(GetGradeById),
                new { id = response.GradeId },
                ApiResponseBuilder.BuildResponse(
                    StatusCodes.Status201Created,
                    "Grade created successfully",
                    response
                )
            );
        }

        [HttpPut(ApiEndPointConstant.Grades.UpdateGradeEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<GradeListResponse      >), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateSubmission(Guid id, [FromForm] UpdateGradeRequest request)
        {
            var updatedGrade = await _gradeService.UpdateGradeAsync(id, request);
            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK,
                "Grade updated successfully",
                updatedGrade
            ));
        }

        [HttpPatch(ApiEndPointConstant.Grades.DeleteGradeEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteGrade(Guid id)
        {
            await _gradeService.DeleteGradeAsync(id);
            return Ok(ApiResponseBuilder.BuildResponse<object>(
                StatusCodes.Status200OK,
                "Grade deleted successfully",
                null
            ));
        }
    }
}
