/*
using System.Linq.Expressions;
using AutoMapper;
using EzyFix.BLL.Extension;
using EzyFix.BLL.Services.Interfaces;
using EzyFix.BLL.Utils;
using EzyFix.DAL.Data.Entities;
using EzyFix.DAL.Data.Exceptions;
using EzyFix.DAL.Data.Requests.Auth;
using EzyFix.DAL.Data.Responses.Auth;
using EzyFix.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace EzyFix.BLL.Services.Implements
{
    public class AuthService : BaseService<AuthService>, IAuthService
    {
        private readonly JwtUtil _jwtUtil;
        private readonly IOtpService _otpService;
        private readonly IRefreshTokensService _refreshTokensService;

        public AuthService(
             IUnitOfWork<EzyFixDbContext> unitOfWork,
             ILogger<AuthService> logger,
             IMapper mapper,
             IHttpContextAccessor httpContextAccessor,
             JwtUtil jwtUtil,
            IOtpService otpService,
            IRefreshTokensService refreshTokensService)
             : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
            _jwtUtil = jwtUtil;
            _otpService = otpService;
            _refreshTokensService = refreshTokensService;
        }

        public async Task<LoginResponse> Login(LoginRequest loginRequest)
        {
            Expression<Func<Staff, bool>> searchEmailAddress = p => p.Email.Equals(loginRequest.Email);

            var staff = (await _unitOfWork.GetRepository<Staff>().SingleOrDefaultAsync(predicate: searchEmailAddress))
                .ValidateExists(customMessage: $"User with email {loginRequest.Email} not found.");

            bool passwordVerify = await PasswordUtil.VerifyPassword(loginRequest.Password, staff.Password)
                ? true
                : throw new WrongPasswordException("Invalid password");

            DateTime? lastChangePassword = staff.LastChangePassword;
            bool isPasswordExpired = lastChangePassword == null || lastChangePassword <= DateTime.UtcNow.AddMonths(-3);

            LoginResponse loginResponse = new LoginResponse(staff);
            loginResponse.IsPasswordExpired = isPasswordExpired;

            Tuple<string, Guid> guidSecurityClaim = new Tuple<string, Guid>("StaffId", staff.Id);

            if (isPasswordExpired)
            {
                // Tạo resetToken cho resetPasswordOnly
                var resetToken = _jwtUtil.GenerateJwtToken(staff, guidSecurityClaim, true);

                // Ném exception với resetToken trong ExceptionMessage
                throw new PasswordExpiredException(resetToken);
            }

            // Token bình thường nếu mật khẩu không hết hạn
            var token = _jwtUtil.GenerateJwtToken(staff, guidSecurityClaim, false);
            var refreshToken = await _refreshTokensService.GenerateAndStoreRefreshToken(staff.Id);

            loginResponse.AccessToken = token;
            loginResponse.RefreshToken = refreshToken; // Thêm refresh token vào response
            return loginResponse;
        }

        public async Task<ForgotPasswordResponse> ForgotPassword(ForgotPasswordRequest forgotPasswordRequest)
        {
            var staffRepository = _unitOfWork.GetRepository<Staff>();

            var staff = (await staffRepository.SingleOrDefaultAsync(
                predicate: s => s.Email == forgotPasswordRequest.Email && s.IsActive
            )).ValidateExists(customMessage: "Staff not found or inactive.");

            var otpValidationResult = await _otpService.ValidateOtp(forgotPasswordRequest.Email, forgotPasswordRequest.Otp);
            if (!otpValidationResult.Success)
            {
                throw new OtpValidationException("Invalid OTP.", otpValidationResult.AttemptsLeft);
            }

            staff.Password = await PasswordUtil.HashPassword(forgotPasswordRequest.NewPassword);
            staff.LastChangePassword = DateTime.UtcNow;
            staffRepository.UpdateAsync(staff);
            await _unitOfWork.CommitAsync();

            return new ForgotPasswordResponse
            {
                Success = true,
                AttemptsLeft = otpValidationResult.AttemptsLeft
            };
        }

        public async Task<ChangePasswordResponse> ChangePassword(ChangePasswordRequest changePasswordRequest)
        {
            var staffRepository = _unitOfWork.GetRepository<Staff>();
            var staff = (await staffRepository.SingleOrDefaultAsync(
                predicate: s => s.Email == changePasswordRequest.Email && s.IsActive
            )).ValidateExists(customMessage: "Staff not found or inactive.");

            var otpRepository = _unitOfWork.GetRepository<Otp>();
            var otpEntity = await otpRepository.SingleOrDefaultAsync(
                predicate: o => o.Email == changePasswordRequest.Email
            );

            int attemptsLeft = otpEntity?.AttemptLeft ?? 0;

            bool oldPasswordVerify = await PasswordUtil.VerifyPassword(changePasswordRequest.OldPassword, staff.Password);
            if (!oldPasswordVerify)
            {
                if (otpEntity != null && otpEntity.AttemptLeft > 0)
                {
                    otpEntity.AttemptLeft -= 1;
                    attemptsLeft = otpEntity.AttemptLeft;

                    if (otpEntity.AttemptLeft <= 0)
                    {
                        otpRepository.DeleteAsync(otpEntity);
                    }
                    else
                    {
                        otpRepository.UpdateAsync(otpEntity);
                    }

                    await _unitOfWork.CommitAsync();
                }

                throw new OtpValidationException("Invalid old password.", attemptsLeft);
            }

            if (changePasswordRequest.NewPassword == changePasswordRequest.OldPassword)
            {
                throw new InvalidOperationException("New password must be different from the old password.");
            }

            var otpValidationResult = await _otpService.ValidateOtp(changePasswordRequest.Email, changePasswordRequest.Otp);
            if (!otpValidationResult.Success)
            {
                throw new OtpValidationException("Invalid OTP.", otpValidationResult.AttemptsLeft);
            }

            staff.Password = await PasswordUtil.HashPassword(changePasswordRequest.NewPassword);
            staff.LastChangePassword = DateTime.UtcNow;

            staffRepository.UpdateAsync(staff);

            await _unitOfWork.CommitAsync();

            return new ChangePasswordResponse
            {
                Success = true,
                AttemptsLeft = otpValidationResult.AttemptsLeft
            };
        }
    }
}
*/
// Các 'using' cần thiết cho Google Login
using AutoMapper;
using EzyFix.BLL.Services.Interfaces;
using EzyFix.BLL.Utils;
using EzyFix.DAL.Data.Exceptions;
using EzyFix.DAL.Data.MetaDatas;
using EzyFix.DAL.Data.Requests.Auth;
using EzyFix.DAL.Data.Responses.Auth;
using EzyFix.DAL.Models;
using EzyFix.DAL.Repositories.Interfaces;
using EzyFix.DAL.Responses;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Security.Authentication;

namespace EzyFix.BLL.Services.Implements
{
    public class AuthService : BaseService<AuthService>, IAuthService
    {
        private readonly IJwtUtil _jwtUtil;
        private readonly IConfiguration _configuration;

        public AuthService(
            IUnitOfWork<AppDbContext> unitOfWork,
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
                // 1. Get Google ClientId from appsettings
                var googleClientId = _configuration["GoogleSettings:ClientId"];

                if (string.IsNullOrEmpty(googleClientId))
                {
                    throw new InvalidOperationException("Google ClientId is not configured.");
                }

                // 2. Validate Google IdToken
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

                // 3. Find or Create User
                var userRepository = _unitOfWork.GetRepository<User>();
                var user = await userRepository.SingleOrDefaultAsync(predicate: u => u.Email == payload.Email);

                // *** FIX 1: Thêm biến cờ để theo dõi người dùng mới ***
                bool isNewUser = false;

                if (user == null)
                {
                    // *** FIX 2: Đặt cờ là true nếu user là null ***
                    isNewUser = true;

                    // Create new user from Google account
                    user = new User
                    {
                        UserId = Guid.NewGuid(),
                        Email = payload.Email,
                        Name = payload.Name ?? payload.Email,
                        EmailConfirmed = payload.EmailVerified,
                        IsActive = true,
                        RoleId = EzyFix.DAL.RoleConstants.DefaultUserRoleId,
                        Password = null, // No password for Google login users
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    await userRepository.InsertAsync(user);
                    await _unitOfWork.CommitAsync();

                    _logger.LogInformation("Created new user from Google login: {Email}", user.Email);
                }

                // 4. Check if account is active
                if (user.IsActive != true)
                {
                    return ApiResponseBuilder.BuildErrorResponse<GoogleLoginResponse>(
                        null,
                        StatusCodes.Status403Forbidden,
                        "Account is inactive",
                        "This account has been disabled"
                    );
                }

                // =================================================================
                var roleRepository = _unitOfWork.GetRepository<Role>(); // Giả sử bạn có model 'Role'
                var userRole = await roleRepository.SingleOrDefaultAsync(predicate: r => r.RoleId == user.RoleId);

                if (userRole == null)
                {
                    // Đây là lỗi nghiêm trọng, RoleId của user không tồn tại trong bảng Role
                    _logger.LogError("Lỗi nghiêm trọng: User {Email} có RoleId {RoleId} không tồn tại trong bảng Role.", user.Email, user.RoleId);
                    return ApiResponseBuilder.BuildErrorResponse<GoogleLoginResponse>(
                        null,
                        StatusCodes.Status500InternalServerError,
                        "Login failed",
                        "Lỗi cấu hình vai trò người dùng."
                    );
                }
                var roleName = userRole.RoleName;

                // 5. Generate JWT Token
                Tuple<string, Guid> guidSecurityClaim = new Tuple<string, Guid>("UserId", user.UserId);
                var accessToken = _jwtUtil.GenerateJwtToken(user, guidSecurityClaim,roleName, false);

                // 6. Create Response
                // *** FIX 3: Sử dụng 'GoogleLoginResponse' thay vì 'LoginResponse' ***
                var googleLoginResponse = new GoogleLoginResponse
                {
                    UserId = user.UserId,
                    Email = user.Email,
                    Name = user.Name,
                    AccessToken = accessToken,
                    RefreshToken = null, // Như logic cũ của bạn
                    IsNewUser = isNewUser // *** FIX 4: Gán cờ người dùng mới ***
                };

                // *** FIX 5: Trả về đối tượng 'googleLoginResponse' đã được tạo ***
                return ApiResponseBuilder.BuildResponse(
                    StatusCodes.Status200OK,
                    "Google login successful",
                    googleLoginResponse // <- Biến này bây giờ có kiểu dữ liệu chính xác
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