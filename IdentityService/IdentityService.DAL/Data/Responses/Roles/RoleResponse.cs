using System;

namespace IdentityService.DAL.Data.Responses.Roles
{
    public class RoleResponse
    {
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
    }
}
