
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Auth0.ManagementApi.Models;
using AutoMapper;
using EzyFix.BLL.Extension;
using EzyFix.BLL.Services.Interfaces;
using EzyFix.BLL.Utils;
// using EzyFix.DAL.Data.Entities;
using EzyFix.DAL.Data.Exceptions;
// using EzyFix.DAL.Data.Requests.Auth;
using EzyFix.DAL.Data.Responses.Auth;
using EzyFix.DAL.Models;
using EzyFix.DAL.Repositories.Interfaces;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace EzyFix.BLL.Services.Implements
{
    public class AuthService : BaseService<AuthService>, IAuthService
    {
        // private readonly JwtUtil _jwtUtil;
        // private readonly IOtpService _otpService;
        // private readonly IRefreshTokensService _refreshTokensService;
        //
        // public AuthService(
        //      IUnitOfWork<EzyFixDbContext> unitOfWork,
        //      ILogger<AuthService> logger,
        //      IMapper mapper,
        //      IHttpContextAccessor httpContextAccessor,
        //      JwtUtil jwtUtil,
        //     IOtpService otpService,
        //     IRefreshTokensService refreshTokensService)
        //      : base(unitOfWork, logger, mapper, httpContextAccessor)
        // {
        //     _jwtUtil = jwtUtil;
        //     _otpService = otpService;
        //     _refreshTokensService = refreshTokensService;
        // }
        //
        // public async Task<LoginResponse> Login(LoginRequest loginRequest)
        // {
        //     Expression<Func<Staff, bool>> searchEmailAddress = p => p.Email.Equals(loginRequest.Email);
        //
        //     var staff = (await _unitOfWork.GetRepository<Staff>().SingleOrDefaultAsync(predicate: searchEmailAddress))
        //         .ValidateExists(customMessage: $"User with email {loginRequest.Email} not found.");
        //
        //     bool passwordVerify = await PasswordUtil.VerifyPassword(loginRequest.Password, staff.Password)
        //         ? true
        //         : throw new WrongPasswordException("Invalid password");
        //
        //     DateTime? lastChangePassword = staff.LastChangePassword;
        //     bool isPasswordExpired = lastChangePassword == null || lastChangePassword <= DateTime.UtcNow.AddMonths(-3);
        //
        //     LoginResponse loginResponse = new LoginResponse(staff);
        //     loginResponse.IsPasswordExpired = isPasswordExpired;
        //
        //     Tuple<string, Guid> guidSecurityClaim = new Tuple<string, Guid>("StaffId", staff.Id);
        //
        //     if (isPasswordExpired)
        //     {
        //         // Tạo resetToken cho resetPasswordOnly
        //         var resetToken = _jwtUtil.GenerateJwtToken(staff, guidSecurityClaim, true);
        //
        //         // Ném exception với resetToken trong ExceptionMessage
        //         throw new PasswordExpiredException(resetToken);
        //     }
        //
        //     // Token bình thường nếu mật khẩu không hết hạn
        //     var token = _jwtUtil.GenerateJwtToken(staff, guidSecurityClaim, false);
        //     var refreshToken = await _refreshTokensService.GenerateAndStoreRefreshToken(staff.Id);
        //
        //     loginResponse.AccessToken = token;
        //     loginResponse.RefreshToken = refreshToken; // Thêm refresh token vào response
        //     return loginResponse;
        // }
        //
        // public async Task<ForgotPasswordResponse> ForgotPassword(ForgotPasswordRequest forgotPasswordRequest)
        // {
        //     var staffRepository = _unitOfWork.GetRepository<Staff>();
        //
        //     var staff = (await staffRepository.SingleOrDefaultAsync(
        //         predicate: s => s.Email == forgotPasswordRequest.Email && s.IsActive
        //     )).ValidateExists(customMessage: "Staff not found or inactive.");
        //
        //     var otpValidationResult = await _otpService.ValidateOtp(forgotPasswordRequest.Email, forgotPasswordRequest.Otp);
        //     if (!otpValidationResult.Success)
        //     {
        //         throw new OtpValidationException("Invalid OTP.", otpValidationResult.AttemptsLeft);
        //     }
        //
        //     staff.Password = await PasswordUtil.HashPassword(forgotPasswordRequest.NewPassword);
        //     staff.LastChangePassword = DateTime.UtcNow;
        //     staffRepository.UpdateAsync(staff);
        //     await _unitOfWork.CommitAsync();
        //
        //     return new ForgotPasswordResponse
        //     {
        //         Success = true,
        //         AttemptsLeft = otpValidationResult.AttemptsLeft
        //     };
        // }
        //
        // public async Task<ChangePasswordResponse> ChangePassword(ChangePasswordRequest changePasswordRequest)
        // {
        //     var staffRepository = _unitOfWork.GetRepository<Staff>();
        //     var staff = (await staffRepository.SingleOrDefaultAsync(
        //         predicate: s => s.Email == changePasswordRequest.Email && s.IsActive
        //     )).ValidateExists(customMessage: "Staff not found or inactive.");
        //
        //     var otpRepository = _unitOfWork.GetRepository<Otp>();
        //     var otpEntity = await otpRepository.SingleOrDefaultAsync(
        //         predicate: o => o.Email == changePasswordRequest.Email
        //     );
        //
        //     int attemptsLeft = otpEntity?.AttemptLeft ?? 0;
        //
        //     bool oldPasswordVerify = await PasswordUtil.VerifyPassword(changePasswordRequest.OldPassword, staff.Password);
        //     if (!oldPasswordVerify)
        //     {
        //         if (otpEntity != null && otpEntity.AttemptLeft > 0)
        //         {
        //             otpEntity.AttemptLeft -= 1;
        //             attemptsLeft = otpEntity.AttemptLeft;
        //
        //             if (otpEntity.AttemptLeft <= 0)
        //             {
        //                 otpRepository.DeleteAsync(otpEntity);
        //             }
        //             else
        //             {
        //                 otpRepository.UpdateAsync(otpEntity);
        //             }
        //
        //             await _unitOfWork.CommitAsync();
        //         }
        //
        //         throw new OtpValidationException("Invalid old password.", attemptsLeft);
        //     }
        //
        //     if (changePasswordRequest.NewPassword == changePasswordRequest.OldPassword)
        //     {
        //         throw new InvalidOperationException("New password must be different from the old password.");
        //     }
        //
        //     var otpValidationResult = await _otpService.ValidateOtp(changePasswordRequest.Email, changePasswordRequest.Otp);
        //     if (!otpValidationResult.Success)
        //     {
        //         throw new OtpValidationException("Invalid OTP.", otpValidationResult.AttemptsLeft);
        //     }
        //
        //     staff.Password = await PasswordUtil.HashPassword(changePasswordRequest.NewPassword);
        //     staff.LastChangePassword = DateTime.UtcNow;
        //
        //     staffRepository.UpdateAsync(staff);
        //
        //     await _unitOfWork.CommitAsync();
        //
        //     return new ChangePasswordResponse
        //     {
        //         Success = true,
        //         AttemptsLeft = otpValidationResult.AttemptsLeft
        //     };
        // }
        
        
        private IConfiguration _configuration;
        private ILectureRepository _userRepo;

        public AuthService(IConfiguration configuration, ILectureRepository userRepo, ILogger<AuthService> logger, IUnitOfWork<AppDbContext> unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base( unitOfWork, logger, mapper, httpContextAccessor)
        {
            _configuration = configuration;
            _userRepo = userRepo;
        }
        public string CreateAccessToken(Lecturer user)
        {
            var key = _configuration["Jwt:Key"];
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var expHours = double.Parse(_configuration["Jwt:AccessTokenExpirationHours"] ?? "24");

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var creds = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.LecturerId),
                new Claim(JwtRegisteredClaimNames.Name, user.Name ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                notBefore: DateTime.UtcNow.AddSeconds(-5),
                expires: DateTime.UtcNow.AddHours(expHours),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string CreateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        
        public async Task<AuthResultResponse> LoginWithGoogleAsync(GoogleLoginDTO googleLoginDto)
        {
            var googleClientId = _configuration["GoogleAuthSettings:ClientId"];
            if (string.IsNullOrEmpty(googleClientId))
            {
                return new AuthResultResponse { Succeeded = false, Message = "Google Client ID is not configured." };
            }

            try
            {
                var validationSettings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { googleClientId }
                };
                var payload = await GoogleJsonWebSignature.ValidateAsync(googleLoginDto.idToken, validationSettings);
                
                var user = await _userRepo.GetUserByEmailAsync(payload.Email); 

                if (user == null)
                {
                    user = new Lecturer
                    {
                        LecturerId = Guid.NewGuid().ToString(),
                        Email = payload.Email,
                        Name = payload.Name, 
                        EmailConfirmed = payload.EmailVerified,
                        Password = null 
                        
                    };
                    user = await _userRepo.CreateUserAsync(user); 
                }

                // Tạo token của hệ thống bạn và trả về
                var accessToken = CreateAccessToken(user);
                var refreshToken = CreateRefreshToken();

                //user.RefreshToken = refreshToken;
                //user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
                await _userRepo.UpdateUserAsync(user);

                return new AuthResultResponse()
                {
                    Succeeded = true,
                    Message = "Google login successful.",
                    Token = accessToken,
                };
            }
            catch (InvalidJwtException ex)
            {
                // Token không hợp lệ
                return new AuthResultResponse() { Succeeded = false, Message = "Invalid Google token." };
            }
            catch (Exception ex)
            {
                // Lỗi không xác định
                return new AuthResultResponse() { Succeeded = false, Message = $"An error occurred: {ex.Message}" };
            }
        }
        
    }
}
