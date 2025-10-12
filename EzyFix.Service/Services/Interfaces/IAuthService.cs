
using EzyFix.DAL.Data.Responses.Auth;

namespace EzyFix.BLL.Services.Interfaces
{
    public interface IAuthService
    {
        // Task<LoginResponse> Login(LoginRequest loginRequest);
        // Task<ForgotPasswordResponse> ForgotPassword(ForgotPasswordRequest forgotPasswordRequest);
        // Task<ChangePasswordResponse> ChangePassword(ChangePasswordRequest changePasswordRequest);
        
        Task<AuthResultResponse> LoginWithGoogleAsync(GoogleLoginDTO googleLoginDto);
    }
}
