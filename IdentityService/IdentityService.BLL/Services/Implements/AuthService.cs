using AutoMapper;
using IdentityService.BLL.Services.Interfaces;
using IdentityService.BLL.Utils;
using IdentityService.DAL.Data.Exceptions;
using IdentityService.DAL.Data.MetaDatas;
using IdentityService.DAL.Data.Requests.Auth;
using IdentityService.DAL.Data.Responses.Auth;
using IdentityService.DAL.Models;
using IdentityService.DAL.Repositories.Interfaces;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace IdentityService.BLL.Services.Implements
{
    public class AuthService : BaseService<AuthService>, IAuthService
    {
        private readonly IJwtUtil _jwtUtil;
        private readonly IConfiguration _configuration;

        public AuthService(
            IUnitOfWork<IdentityDbContext> unitOfWork,
            ILogger<AuthService> logger,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IJwtUtil jwtUtil,
            IConfiguration configuration)
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
            _jwtUtil = jwtUtil;
            _configuration = configuration;
        }

        public async Task<ApiResponse<GoogleLoginResponse>> GoogleLoginAsync(GoogleLoginRequest request)
        {
            try
            {
                var googleClientId = _configuration["GoogleSettings:ClientId"];

                if (string.IsNullOrEmpty(googleClientId))
                {
                    throw new InvalidOperationException("Google ClientId is not configured.");
                }

                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new List<string> { googleClientId }
                };

                GoogleJsonWebSignature.Payload payload;
                try
                {
                    payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken, settings);
                }
                catch (InvalidJwtException ex)
                {
                    _logger.LogWarning(ex, "Invalid Google IdToken received");
                    return ApiResponseBuilder.BuildErrorResponse<GoogleLoginResponse>(
                        null,
                        StatusCodes.Status401Unauthorized,
                        "Invalid Google token",
                        "The provided Google token is invalid or expired"
                    );
                }

                var userRepository = _unitOfWork.GetRepository<User>();
                var user = await userRepository.SingleOrDefaultAsync(predicate: u => u.Email == payload.Email);

                bool isNewUser = false;

                if (user == null)
                {
                    isNewUser = true;

                    user = new User
                    {
                        UserId = Guid.NewGuid(),
                        Email = payload.Email,
                        Name = payload.Name ?? payload.Email,
                        EmailConfirmed = payload.EmailVerified,
                        IsActive = true,
                        RoleId = IdentityService.DAL.RoleConstants.ExaminerRoleId,
                        Password = null,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    await userRepository.InsertAsync(user);
                    await _unitOfWork.CommitAsync();

                    _logger.LogInformation("Created new user from Google login: {Email}", user.Email);
                }

                if (user.IsActive != true)
                {
                    return ApiResponseBuilder.BuildErrorResponse<GoogleLoginResponse>(
                        null,
                        StatusCodes.Status403Forbidden,
                        "Account is inactive",
                        "This account has been disabled"
                    );
                }

                var roleRepository = _unitOfWork.GetRepository<Role>();
                var userRole = await roleRepository.SingleOrDefaultAsync(predicate: r => r.RoleId == user.RoleId);

                if (userRole == null)
                {
                    _logger.LogError("Critical error: User {Email} has RoleId {RoleId} which doesn't exist in Role table.", user.Email, user.RoleId);
                    return ApiResponseBuilder.BuildErrorResponse<GoogleLoginResponse>(
                        null,
                        StatusCodes.Status500InternalServerError,
                        "Login failed",
                        "User role configuration error."
                    );
                }
                
                var roleName = userRole.RoleName;

                Tuple<string, Guid> guidSecurityClaim = new Tuple<string, Guid>("UserId", user.UserId);
                var accessToken = _jwtUtil.GenerateJwtToken(user, guidSecurityClaim, roleName, false);

                var googleLoginResponse = new GoogleLoginResponse
                {
                    UserId = user.UserId,
                    Email = user.Email,
                    Name = user.Name,
                    AccessToken = accessToken,
                    RefreshToken = null,
                    IsNewUser = isNewUser
                };

                return ApiResponseBuilder.BuildResponse(
                    StatusCodes.Status200OK,
                    "Google login successful",
                    googleLoginResponse
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during Google login process");
                return ApiResponseBuilder.BuildErrorResponse<GoogleLoginResponse>(
                    null,
                    StatusCodes.Status500InternalServerError,
                    "Login failed",
                    ex.Message
                );
            }
        }
    }
}
