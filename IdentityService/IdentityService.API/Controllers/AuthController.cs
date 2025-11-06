using IdentityService.API.Constants;
using IdentityService.BLL.Services.Interfaces;
using IdentityService.DAL.Data.MetaDatas;
using IdentityService.DAL.Data.Requests.Auth;
using IdentityService.DAL.Data.Responses.Auth;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.API.Controllers
{
    [ApiController]
    public class AuthController : BaseController<AuthController>
    {
        private readonly IAuthService _authService;
        private readonly IRefreshTokensService _refreshTokensService;
        
        public AuthController(
            ILogger<AuthController> logger, 
            IAuthService authService, 
            IRefreshTokensService refreshTokensService) 
            : base(logger)
        {
            _authService = authService;
            _refreshTokensService = refreshTokensService;
        }

        [HttpPost(ApiEndPointConstant.Auth.GoogleLoginEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<GoogleLoginResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(
                    ApiResponseBuilder.BuildErrorResponse<object>(
                        null,
                        StatusCodes.Status400BadRequest,
                        "Invalid request",
                        "Please provide a valid Google ID token."
                    )
                );
            }

            var response = await _authService.GoogleLoginAsync(request);
            
            if (response.StatusCode == StatusCodes.Status200OK)
            {
                return Ok(response);
            }
            
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost(ApiEndPointConstant.Auth.RefreshTokenEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                var newAccessToken = await _refreshTokensService.RefreshAccessToken(request.RefreshToken);
                return Ok(ApiResponseBuilder.BuildResponse(
                    StatusCodes.Status200OK,
                    "Token refreshed successfully",
                    new { accessToken = newAccessToken }
                ));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error refreshing token: {Message}", ex.Message);
                return Unauthorized(ApiResponseBuilder.BuildErrorResponse<object>(
                    null,
                    StatusCodes.Status401Unauthorized,
                    "Token refresh failed",
                    ex.Message
                ));
            }
        }

        [HttpDelete(ApiEndPointConstant.Auth.DeleteRefreshTokenEndpoint)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteRefreshToken([FromQuery] string refreshToken)
        {
            var result = await _refreshTokensService.DeleteRefreshToken(refreshToken);
            
            if (result)
            {
                return NoContent();
            }
            
            return BadRequest(ApiResponseBuilder.BuildErrorResponse<object>(
                null,
                StatusCodes.Status400BadRequest,
                "Delete failed",
                "Refresh token not found"
            ));
        }
    }
}
