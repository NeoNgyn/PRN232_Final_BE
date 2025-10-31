using Auth0.ManagementApi.Models.Rules;
using EzyFix.DAL.Data.MetaDatas;
using EzyFix.DAL.Data.Requests.Auth;
using EzyFix.DAL.Data.Responses.Auth;
using EzyFix.DAL.Responses;


namespace EzyFix.BLL.Services.Interfaces
{
    public interface IAuthService
    {
        //Task<LoginResponse> Login(LoginRequest loginRequest);
        //Task<ForgotPasswordResponse> ForgotPassword(ForgotPasswordRequest forgotPasswordRequest);
        //Task<ChangePasswordResponse> ChangePassword(ChangePasswordRequest changePasswordRequest);

        // Thêm ph??ng th?c Google Login
        Task<ApiResponse<GoogleLoginResponse>> GoogleLoginAsync(GoogleLoginRequest request);
    }
}