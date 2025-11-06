using System.ComponentModel.DataAnnotations;

namespace IdentityService.DAL.Data.Requests.Roles
{
    public class CreateRoleRequest
    {
        [Required(ErrorMessage = "Role name is required")]
        [StringLength(50, ErrorMessage = "Role name cannot exceed 50 characters")]
        public string RoleName { get; set; }

        [StringLength(200, ErrorMessage = "Description cannot exceed 200 characters")]
        public string Description { get; set; }
    }
}
