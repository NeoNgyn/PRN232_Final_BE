using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzyFix.DAL.Models
{
    public class Role
    {
        public Guid RoleId { get; set; }   // Khóa chính (UUID)

        public string RoleName { get; set; }  // Tên vai trò, ví dụ: "Admin", "Lecturer", "Student"

        public string Description { get; set; }  // Mô tả vai trò (tùy chọn)

        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}
