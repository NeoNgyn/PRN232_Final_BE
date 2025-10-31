using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EzyFix.DAL.Models;

namespace EzyFix.BLL.Utils
{
    public interface IJwtUtil
    {
        string GenerateJwtToken(User user, Tuple<string, Guid> tuple,string roleName, bool flag);
    }
}
