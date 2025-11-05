using IdentityService.DAL.Data.Requests.Roles;
using IdentityService.DAL.Data.Responses.Roles;

namespace IdentityService.BLL.Services.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleResponse>> GetAllRolesAsync();
        Task<RoleResponse?> GetRoleByIdAsync(Guid id);
        Task<RoleResponse> CreateRoleAsync(CreateRoleRequest createDto);
        Task<RoleResponse> UpdateRoleAsync(Guid id, UpdateRoleRequest updateDto);
        Task<bool> DeleteRoleAsync(Guid id);
    }
}
