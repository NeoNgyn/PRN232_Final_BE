using EzyFix.DAL.Data.Requests.Files;
using EzyFix.DAL.Data.Requests.Roles;
using EzyFix.DAL.Data.Responses.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzyFix.BLL.Services.Interfaces
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
