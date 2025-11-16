using AcademicService.API.Constants;
using AcademicService.BLL.Services.Interfaces;
using AcademicService.DAL.Data.MetaDatas;
using AcademicService.DAL.Data.Requests.Submission;
using AcademicService.DAL.Data.Responses.Submission;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace AcademicService.API.Controllers
{
    [ApiController]
    public class SubmissionController : BaseController<SubmissionController>
    {
        private readonly ISubmissionService _submissionService;
        private readonly IMapper _mapper;

        public SubmissionController(ILogger<SubmissionController> logger, ISubmissionService submissionService, IMapper mapper) : base(logger)
        {
            _submissionService = submissionService;
            _mapper = mapper;
        }

        [HttpGet(ApiEndPointConstant.Submissions.SubmissionsEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<SubmissionListResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllSubmissions()
        {
            var userClaims = User.Claims.Select(c => new { c.Type, c.Value }); //debugging code
            _logger.LogInformation("User claims: {@Claims}", userClaims);

            var submissions = await _submissionService.GetAllSubmissionsAsync();
            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK,
                "Submission list retrieved successfully",
                submissions
            ));
        }

        [HttpGet(ApiEndPointConstant.Submissions.SubmissionEndpointByExamId)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<SubmissionListResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllSubmissionsByExam(Guid examId)
        {
            var userClaims = User.Claims.Select(c => new { c.Type, c.Value }); //debugging code
            _logger.LogInformation("User claims: {@Claims}", userClaims);

            var submissions = await _submissionService.GetSubmissionsByExamIdAsync(examId);
            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK,
                "Submission list retrieved successfully",
                submissions
            ));
        }

        [HttpGet(ApiEndPointConstant.Submissions.SubmissionEndpointByExamIdAndExaminerId)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<SubmissionListResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllSubmissionsByExamAndExanminer(Guid examId, Guid examinerId)
        {
            var userClaims = User.Claims.Select(c => new { c.Type, c.Value }); //debugging code
            _logger.LogInformation("User claims: {@Claims}", userClaims);

            var submissions = await _submissionService.GetSubmissionsByExamIdAndExamninerIdAsync(examId, examinerId);
            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK,
                "Submission list retrieved successfully",
                submissions
            ));
        }

        [HttpGet(ApiEndPointConstant.Submissions.QuerySubmissionEndpoint)]
        [EnableQuery]
        public IActionResult QuerySubmissions()
        {
            var query = _submissionService.GetQuerySubmissions()
                .ProjectTo<SubmissionListResponse>(_mapper.ConfigurationProvider);

            return Ok(query);
        }


        [HttpGet(ApiEndPointConstant.Submissions.SubmissionEndpointById)]
        [ProducesResponseType(typeof(ApiResponse<SubmissionDetailResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSubmissionById(Guid id)
        {
            var submission = await _submissionService.GetSubmissionByIdAsync(id);
            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK,
                "Submission retrieved successfully",
                submission
            ));
        }

        [HttpPost(ApiEndPointConstant.Submissions.SubmissionsEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<SubmissionListResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateSubmission([FromBody] CreateSubmissionRequest request,  IFormFile fileSubmit)
        {
            var response = await _submissionService.CreateSubmissionAsync(request, fileSubmit);

            if (response == null)
            {
                return BadRequest(
                    ApiResponseBuilder.BuildErrorResponse<object>(
                        null,
                        StatusCodes.Status400BadRequest,
                        "Failed to create submission",
                        "The submission creation process failed"
                    )
                );
            }

            return CreatedAtAction(
                nameof(GetSubmissionById),
                new { id = response.SubmissionId },
                ApiResponseBuilder.BuildResponse(
                    StatusCodes.Status201Created,
                    "Submission created successfully",
                    response
                )
            );
        }

        [HttpPut(ApiEndPointConstant.Submissions.UpdateSubmissionEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<SubmissionListResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateSubmission(Guid id, [FromBody] UpdateSubmissionRequest request)
        {
            var updatedSubmission = await _submissionService.UpdateSubmissionAsync(id, request);
            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK,
                "Submission updated successfully",
                updatedSubmission
            ));
        }

        [HttpDelete(ApiEndPointConstant.Submissions.DeleteSubmissionEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteSubmission(Guid id)
        {
            await _submissionService.DeleteSubmissionAsync(id);
            return Ok(ApiResponseBuilder.BuildResponse<object>(
                StatusCodes.Status200OK,
                "Submission deleted successfully",
                null
            ));
        }
    }
}
