namespace IdentityService.DAL.Data.Requests.Users;

public class UpdateUserRequest
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public bool? IsActive { get; set; }
    public Guid? RoleId { get; set; }
}