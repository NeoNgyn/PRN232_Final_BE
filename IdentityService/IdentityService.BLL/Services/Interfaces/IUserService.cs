using IdentityService.DAL.Data.Requests.Users;
using IdentityService.DAL.Data.Responses.Users;

namespace IdentityService.BLL.Services.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserResponse>> GetAllAsync();
    Task<IEnumerable<UserResponse>> GetTeachersAsync();
    Task<UserResponse> GetByIdAsync(Guid id);
    Task<UserResponse> CreateAsync(CreateUserRequest request);
    Task<UserResponse> UpdateAsync(Guid id, UpdateUserRequest request);
    Task<bool> DeleteAsync(Guid id);
}