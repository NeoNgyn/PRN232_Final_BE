using IdentityService.API.Constants;
using IdentityService.BLL.Services.Interfaces;
using IdentityService.DAL.Data.MetaDatas;
using IdentityService.DAL.Data.Requests.Roles;
using IdentityService.DAL.Data.Responses.Roles;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.API.Controllers
{
    [ApiController]
    public class RolesController : BaseController<RolesController>
    {
        private readonly IRoleService _roleService;

        public RolesController(ILogger<RolesController> logger, IRoleService roleService)
            : base(logger)
        {
            _roleService = roleService;
        }

        [HttpGet(ApiEndPointConstant.Roles.RolesEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<RoleResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleService.GetAllRolesAsync();
            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK,
                "Role list retrieved successfully",
                roles
            ));
        }

        [HttpGet(ApiEndPointConstant.Roles.RoleEndpointById)]
        [ProducesResponseType(typeof(ApiResponse<RoleResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRoleById(Guid id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK,
                "Role retrieved successfully",
                role
            ));
        }

        [HttpPost(ApiEndPointConstant.Roles.RolesEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<RoleResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest request)
        {
            var response = await _roleService.CreateRoleAsync(request);

            if (response == null)
            {
                return BadRequest(ApiResponseBuilder.BuildErrorResponse<object>(
                    null,
                    StatusCodes.Status400BadRequest,
                    "Cannot create new role",
                    "Role creation failed"
                ));
            }

            return CreatedAtAction(
                nameof(GetRoleById),
                new { id = response.RoleId },
                ApiResponseBuilder.BuildResponse(
                    StatusCodes.Status201Created,
                    "Role created successfully",
                    response
                )
            );
        }

        [HttpPut(ApiEndPointConstant.Roles.UpdateRoleEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<RoleResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateRole(Guid id, [FromBody] UpdateRoleRequest request)
        {
            var updated = await _roleService.UpdateRoleAsync(id, request);
            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK,
                "Role updated successfully",
                updated
            ));
        }

        [HttpDelete(ApiEndPointConstant.Roles.DeleteRoleEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteRole(Guid id)
        {
            await _roleService.DeleteRoleAsync(id);
            return Ok(ApiResponseBuilder.BuildResponse<object>(
                StatusCodes.Status200OK,
                "Role deleted successfully",
                null
            ));
        }
    }
}
