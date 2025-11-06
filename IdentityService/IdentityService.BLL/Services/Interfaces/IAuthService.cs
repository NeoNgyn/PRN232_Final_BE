using IdentityService.DAL.Data.MetaDatas;
using IdentityService.DAL.Data.Requests.Auth;
using IdentityService.DAL.Data.Responses.Auth;

namespace IdentityService.BLL.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ApiResponse<GoogleLoginResponse>> GoogleLoginAsync(GoogleLoginRequest request);
    }
}
