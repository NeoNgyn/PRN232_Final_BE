using AutoMapper;
using EzyFix.BLL.Services.Interfaces;
using EzyFix.DAL.Data.Exceptions;
using EzyFix.DAL.Data.Requests.Roles;
using EzyFix.DAL.Data.Responses.Roles;
using EzyFix.DAL.Models;
using EzyFix.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace EzyFix.BLL.Services.Implements
{
    public class RoleService : BaseService<RoleService>, IRoleService
    {
        public RoleService(
            IUnitOfWork<AppDbContext> unitOfWork,
            ILogger<RoleService> logger,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<IEnumerable<RoleResponse>> GetAllRolesAsync()
        {
            try
            {
                var roles = await _unitOfWork.GetRepository<Role>().GetListAsync();
                return _mapper.Map<IEnumerable<RoleResponse>>(roles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving role list: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<RoleResponse?> GetRoleByIdAsync(Guid id)
        {
            try
            {
                var role = await _unitOfWork.GetRepository<Role>()
                    .SingleOrDefaultAsync(predicate: r => r.RoleId == id);

                if (role == null)
                {
                    throw new NotFoundException($"Role with ID {id} not found");
                }

                return _mapper.Map<RoleResponse>(role);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving role by ID {Id}: {Message}", id, ex.Message);
                throw;
            }
        }

        public async Task<RoleResponse> CreateRoleAsync(CreateRoleRequest createDto)
        {
            try
            {
                return await _unitOfWork.ProcessInTransactionAsync(async () =>
                {
                    var role = _mapper.Map<Role>(createDto);
                    role.RoleId = Guid.NewGuid();

                    await _unitOfWork.GetRepository<Role>().InsertAsync(role);
                    await _unitOfWork.CommitAsync();

                    return _mapper.Map<RoleResponse>(role);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating new role: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<RoleResponse> UpdateRoleAsync(Guid id, UpdateRoleRequest updateDto)
        {
            try
            {
                return await _unitOfWork.ProcessInTransactionAsync(async () =>
                {
                    var role = await _unitOfWork.GetRepository<Role>()
                        .SingleOrDefaultAsync(predicate: r => r.RoleId == id);

                    if (role == null)
                    {
                        throw new NotFoundException($"Role with ID {id} not found for update");
                    }

                    _mapper.Map(updateDto, role);
                    _unitOfWork.GetRepository<Role>().UpdateAsync(role);
                    await _unitOfWork.CommitAsync();

                    return _mapper.Map<RoleResponse>(role);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating role {Id}: {Message}", id, ex.Message);
                throw;
            }
        }

        public async Task<bool> DeleteRoleAsync(Guid id)
        {
            try
            {
                return await _unitOfWork.ProcessInTransactionAsync(async () =>
                {
                    var role = await _unitOfWork.GetRepository<Role>()
                        .SingleOrDefaultAsync(predicate: r => r.RoleId == id);

                    if (role == null)
                    {
                        throw new NotFoundException($"Role with ID {id} not found for deletion");
                    }

                    _unitOfWork.GetRepository<Role>().DeleteAsync(role);
                    await _unitOfWork.CommitAsync();
                    return true;
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting role {Id}: {Message}", id, ex.Message);
                throw;
            }
        }

    }
}
