#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityService.DAL.Models
{
    public partial class User
    {
        public Guid UserId { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string LecturerCode { get; set; }

        public bool EmailConfirmed { get; set; }

        public bool? IsActive { get; set; }
        
        [Column(TypeName = "timestamp with time zone")]
        public DateTime? CreatedAt { get; set; }
        
        [Column(TypeName = "timestamp with time zone")]
        public DateTime? UpdatedAt { get; set; }

        public Guid? RoleId { get; set; }

        public virtual Role Role { get; set; }
    }
}
