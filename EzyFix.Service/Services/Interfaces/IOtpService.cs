using EzyFix.DAL.Data.Responses.Otp;

namespace EzyFix.BLL.Services.Interfaces
{
    public interface IOtpService
    {
        Task CreateOtpEntity(string email, string otp);
        Task<ValidateOtpResponse> ValidateOtp(string email, string otp);
    }
}
