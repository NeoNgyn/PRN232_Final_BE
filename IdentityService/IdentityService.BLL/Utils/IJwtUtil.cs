using IdentityService.DAL.Models;

namespace IdentityService.BLL.Utils
{
    public interface IJwtUtil
    {
        string GenerateJwtToken(User user, Tuple<string, Guid> tuple, string roleName, bool flag);
    }
}
