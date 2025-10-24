using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzyFix.DAL.Data.Responses.Roles
{
    public class RoleResponse
    {
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
    }
}
