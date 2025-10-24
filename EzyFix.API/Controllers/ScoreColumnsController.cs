using EzyFix.API.Constants;
using EzyFix.BLL.Services.Interfaces;
using EzyFix.DAL.Data.MetaDatas;
using EzyFix.DAL.Data.Requests.ScoreColumns;
using EzyFix.DAL.Data.Responses.ScoreColumns;
using Microsoft.AspNetCore.Mvc;

namespace EzyFix.API.Controllers
{
    [ApiController]
    public class ScoreColumnsController : BaseController<ScoreColumnsController>
    {
        private readonly IScoreColumnService _scoreColumnService;

        public ScoreColumnsController(ILogger<ScoreColumnsController> logger, IScoreColumnService scoreColumnService) : base(logger)
        {
            _scoreColumnService = scoreColumnService;
        }

        [HttpGet(ApiEndPointConstant.ScoreColumns.ScoreColumnsEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ScoreColumnResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllScoreColumns()
        {
            var scoreColumns = await _scoreColumnService.GetAllScoreColumnsAsync();
            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK, "Score column list retrieved successfully", scoreColumns
            ));
        }

        [HttpGet(ApiEndPointConstant.ScoreColumns.ScoreColumnEndpointById)]
        [ProducesResponseType(typeof(ApiResponse<ScoreColumnResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetScoreColumnById(Guid id)
        {
            var scoreColumn = await _scoreColumnService.GetScoreColumnByIdAsync(id);
            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK, "Score column information retrieved successfully", scoreColumn
            ));
        }

        [HttpPost(ApiEndPointConstant.ScoreColumns.ScoreColumnsEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<ScoreColumnResponse>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateScoreColumn([FromForm] CreateScoreColumnRequest createDto)
        {
            var newScoreColumn = await _scoreColumnService.CreateScoreColumnAsync(createDto);
            return CreatedAtAction(
                nameof(GetScoreColumnById),
                new { id = newScoreColumn.ColumnId },
                ApiResponseBuilder.BuildResponse(
                    StatusCodes.Status201Created, "Score column created successfully", newScoreColumn
                )
            );
        }

        [HttpPut(ApiEndPointConstant.ScoreColumns.UpdateScoreColumnEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<ScoreColumnResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateScoreColumn(Guid id, [FromForm] UpdateScoreColumnRequest updateDto)
        {
            var updatedScoreColumn = await _scoreColumnService.UpdateScoreColumnAsync(id, updateDto);
            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK, "Score column updated successfully", updatedScoreColumn
            ));
        }



        [HttpDelete(ApiEndPointConstant.ScoreColumns.DeleteScoreColumnEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteScoreColumn(Guid id)
        {
            await _scoreColumnService.DeleteScoreColumnAsync(id);
            return Ok(ApiResponseBuilder.BuildResponse<object>(
                StatusCodes.Status200OK, "Score column deleted successfully", null
            ));
        }
    }
}
