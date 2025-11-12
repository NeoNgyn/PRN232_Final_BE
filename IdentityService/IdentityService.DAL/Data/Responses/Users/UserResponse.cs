namespace IdentityService.DAL.Data.Responses.Users;

public class UserResponse
{
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string LecturerCode { get; set; }
    public bool EmailConfirmed { get; set; }
    public bool? IsActive { get; set; }
    public string? RoleName { get; set; }
    public DateTime? CreatedAt { get; set; }
}