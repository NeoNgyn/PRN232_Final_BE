using System;

namespace IdentityService.DAL.Data.Responses.Auth
{
    public class GoogleLoginResponse
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public bool IsNewUser { get; set; }
    }
}
