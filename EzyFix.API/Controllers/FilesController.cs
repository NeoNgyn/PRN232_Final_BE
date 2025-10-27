using EzyFix.API.Constants;
using EzyFix.BLL.Services.Interfaces;
using EzyFix.DAL.Data.MetaDatas;
using EzyFix.DAL.Data.Responses.Assignments;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using System.Linq;

namespace EzyFix.API.Controllers
{
    [ApiController]
    public class FilesController : BaseController<FilesController>
    {
        private readonly IFileService _fileService;
        private readonly IAssignmentService _assignmentService;

        public FilesController(
            ILogger<FilesController> logger,
            IFileService fileService,
            IAssignmentService assignmentService)
            : base(logger)
        {
            _fileService = fileService;
            _assignmentService = assignmentService;
        }

        // ===============================
        // Export All Assignments with OData Support
        // ===============================
        [HttpGet(ApiEndPointConstant.Files.FilesEndpoint + "/export/assignments")]
        [EnableQuery]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ExportAssignments(ODataQueryOptions<AssignmentResponse> queryOptions)
        {
            try
            {
                // Get all assignments
                var assignments = await _assignmentService.GetAllAssignmentsAsync();

                if (!assignments.Any())
                {
                    return NotFound(ApiResponseBuilder.BuildErrorResponse<object>(
                        null,
                        StatusCodes.Status404NotFound,
                        "No assignments found to export",
                        "No data available"
                    ));
                }

                // Apply OData query options to filter the assignments
                var queryable = assignments.AsQueryable();
                var filteredAssignments = queryOptions.ApplyTo(queryable) as IQueryable<AssignmentResponse>;
                var resultAssignments = filteredAssignments.ToList();

                if (!resultAssignments.Any())
                {
                    return NotFound(ApiResponseBuilder.BuildErrorResponse<object>(
                        null,
                        StatusCodes.Status404NotFound,
                        "No assignments found after applying filters",
                        "No data available"
                    ));
                }

                // Export the filtered assignments
                var fileBytes = await _fileService.ExportAssignmentsToExcel(resultAssignments);

                // Add filter info to filename if filters were applied
                string filenamePrefix = queryOptions.Filter != null ? "Filtered_" : "";

                return File(
                    fileBytes,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"{filenamePrefix}Assignments_Export_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting assignments");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponseBuilder.BuildErrorResponse<object>(
                        null,
                        StatusCodes.Status500InternalServerError,
                        "Failed to export assignments",
                        "Internal server error"
                    ));
            }
        }

        // ===============================
        // Export Assignments by Exam with OData
        // ===============================
        [HttpGet(ApiEndPointConstant.Files.FilesEndpoint + "/export/assignments/exam/{examId}")]
        [EnableQuery]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ExportAssignmentsByExam(Guid examId, ODataQueryOptions<AssignmentResponse> queryOptions)
        {
            try
            {
                var assignments = await _assignmentService.GetAssignmentsByExamAsync(examId);

                if (!assignments.Any())
                {
                    return NotFound(ApiResponseBuilder.BuildErrorResponse<object>(
                        null,
                        StatusCodes.Status404NotFound,
                        $"No assignments found for exam ID: {examId}",
                        "No data available"
                    ));
                }

                // Apply OData query options
                var queryable = assignments.AsQueryable();
                var filteredAssignments = queryOptions.ApplyTo(queryable) as IQueryable<AssignmentResponse>;
                var resultAssignments = filteredAssignments.ToList();

                if (!resultAssignments.Any())
                {
                    return NotFound(ApiResponseBuilder.BuildErrorResponse<object>(
                        null,
                        StatusCodes.Status404NotFound,
                        "No assignments found after applying filters",
                        "No data available"
                    ));
                }

                var fileBytes = await _fileService.ExportAssignmentsToExcel(resultAssignments);

                string filenamePrefix = queryOptions.Filter != null ? "Filtered_" : "";

                return File(
                    fileBytes,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"{filenamePrefix}Exam_{examId}_Assignments_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting assignments for exam {ExamId}", examId);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponseBuilder.BuildErrorResponse<object>(
                        null,
                        StatusCodes.Status500InternalServerError,
                        "Failed to export assignments",
                        "Internal server error"
                    ));
            }
        }
    }
}
