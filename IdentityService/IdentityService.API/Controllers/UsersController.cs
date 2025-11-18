using IdentityService.API.Constants;
using IdentityService.BLL.Services.Interfaces;
using IdentityService.DAL.Data.MetaDatas;
using IdentityService.DAL.Data.Requests.Users;
using IdentityService.DAL.Data.Responses.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace IdentityService.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UsersController : BaseController<UsersController>
    {
        private readonly IUserService _userService;

        public UsersController(ILogger<UsersController> logger, IUserService userService) : base(logger)
        {
            _userService = userService;
        }
        
        [HttpGet(ApiEndPointConstant.Users.UsersEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<UserResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        //[Authorize(Roles = "Admin")]
        [EnableQuery(MaxExpansionDepth = 5, PageSize = 50)]
        public async Task<IActionResult> GetUsers()
        {
                var userClaims = User.Claims.Select(c => new { c.Type, c.Value });
                _logger.LogInformation("User claims: {@Claims}", userClaims);

                var users = await _userService.GetAllAsync();
                return Ok(ApiResponseBuilder.BuildResponse(
                    StatusCodes.Status200OK,
                    "User list retrieved successfully",
                    users.AsQueryable()
                ));
        }
        
        [HttpGet(ApiEndPointConstant.Users.UserEndpointById)]
        [ProducesResponseType(typeof(ApiResponse<UserResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _userService.GetByIdAsync(id);
            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK,
                "User retrieved successfully",
                user
            ));
        }
        
        [HttpPost(ApiEndPointConstant.Users.UsersEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<UserResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            var response = await _userService.CreateAsync(request);

            if (response == null)
            {
                return BadRequest(ApiResponseBuilder.BuildErrorResponse<object>(
                    null,
                    StatusCodes.Status400BadRequest,
                    "Failed to create user",
                    "The user creation process failed"
                ));
            }

            return CreatedAtAction(
                nameof(GetUserById),
                new { id = response.UserId },
                ApiResponseBuilder.BuildResponse(
                    StatusCodes.Status201Created,
                    "User created successfully",
                    response
                )
            );
        }
        
        [HttpPut(ApiEndPointConstant.Users.UpdateUserEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<UserResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserRequest request)
        {
            var updatedUser = await _userService.UpdateAsync(id, request);
            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK,
                "User updated successfully",
                updatedUser
            ));
        }
        
        [HttpPut(ApiEndPointConstant.Users.DeleteUserEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            await _userService.DeleteAsync(id);
            return Ok(ApiResponseBuilder.BuildResponse<object>(
                StatusCodes.Status200OK,
                "User deleted successfully",
                null
            ));
        }
        
        [HttpGet("teachers")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<UserResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTeachers()
        {
            var teachers = await _userService.GetTeachersAsync();           
            
            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK,
                "Teachers list retrieved successfully",
                teachers
            ));
        }
    }
}