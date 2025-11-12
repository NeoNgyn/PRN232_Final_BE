namespace IdentityService.DAL.Data.Requests.Users;

public class CreateUserRequest
{
    public string Name { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string LecturerCode { get; set; }
    public Guid? RoleId { get; set; }
}