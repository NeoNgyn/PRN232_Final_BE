using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityService.DAL.Models
{
    [Table("Roles")]
    public class Role
    {
        public Guid RoleId { get; set; }

        public string RoleName { get; set; }

        public string Description { get; set; }

        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}
