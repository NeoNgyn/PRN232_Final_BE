using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EzyFix.DAL.Data.Entities;

namespace EzyFix.BLL.Utils
{
    public interface IJwtUtil
    {
        string GenerateJwtToken(Users user, Tuple<string, Guid> tuple, bool flag);
    }
}
