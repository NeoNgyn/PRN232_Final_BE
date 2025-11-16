using AutoMapper;
using IdentityService.BLL.Extension;
using IdentityService.BLL.Services.Interfaces;
using IdentityService.DAL.Data.Exceptions;
using IdentityService.DAL.Data.Requests.Users;
using IdentityService.DAL.Data.Responses.Users;
using IdentityService.DAL.Models;
using IdentityService.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace IdentityService.BLL.Services.Implements
{
    public class UserService : BaseService<UserService>, IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        public UserService(IUnitOfWork<IdentityDbContext> unitOfWork, ILogger<UserService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<UserResponse>> GetAllAsync()
        {
            try
            {
                var users = await _unitOfWork.GetRepository<User>()
                    .GetListAsync(
                        predicate: u => u.IsActive == true,
                        include: q => q.Include(u => u.Role)
                    );

                return _mapper.Map<IEnumerable<UserResponse>>(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user list: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<UserResponse> GetByIdAsync(Guid id)
        {
            try
            {
                var user = (await _unitOfWork.GetRepository<User>()
                    .SingleOrDefaultAsync(
                        predicate: u => u.UserId == id && u.IsActive == true,
                        include: q => q.Include(u => u.Role)
                    )).ValidateExists(id, "User not found.");

                return _mapper.Map<UserResponse>(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user by ID: {Message}", ex.Message);
                throw;
            }
        }
        
        public async Task<UserResponse> CreateAsync(CreateUserRequest createUserRequest)
        {
            try
            {
                if (createUserRequest == null)
                    throw new ArgumentNullException(nameof(createUserRequest), "Request cannot be null.");

                if (string.IsNullOrWhiteSpace(createUserRequest.Email))
                    throw new BadRequestException("Email cannot be empty.");

                if (createUserRequest.Email.Length > 256)
                    throw new BadRequestException("Email cannot exceed 256 characters.");

                if (string.IsNullOrWhiteSpace(createUserRequest.Password))
                {
                    _logger.LogInformation("No password provided for {Email}, using default password.", createUserRequest.Email);
                    createUserRequest.Password = "123456@";
                }
                
                if (createUserRequest.Password.Length < 6)
                    throw new BadRequestException("Password must be at least 6 characters.");

                return await _unitOfWork.ProcessInTransactionAsync(async () =>
                {
                    var userRepo = _unitOfWork.GetRepository<User>();

                    var existing = await userRepo.SingleOrDefaultAsync(predicate: s => s.Email == createUserRequest.Email);
                    if (existing != null)
                        throw new BusinessException("Email already exists.");

                    var newUser = _mapper.Map<User>(createUserRequest);
                    newUser.UserId = Guid.NewGuid();
                    newUser.EmailConfirmed = false;
                    newUser.IsActive = true;
                    newUser.CreatedAt = DateTime.UtcNow;
                    
                    newUser.Password = BCrypt.Net.BCrypt.HashPassword(createUserRequest.Password);

                    await userRepo.InsertAsync(newUser);
                    await _unitOfWork.CommitAsync();

                    var createdUser = await userRepo.SingleOrDefaultAsync(
                        predicate: u => u.UserId == newUser.UserId,
                        include: q => q.Include(u => u.Role)
                    );
                    
                    return _mapper.Map<UserResponse>(newUser);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<UserResponse> UpdateAsync(Guid id, UpdateUserRequest request)
        {
            try
            {
                if (request == null)
                    throw new ArgumentNullException(nameof(request), "Request cannot be null.");

                if (string.IsNullOrWhiteSpace(request.Email))
                    throw new BadRequestException("Email cannot be empty.");

                if (request.Email.Length > 256)
                    throw new BadRequestException("Email cannot exceed 256 characters.");

                return await _unitOfWork.ProcessInTransactionAsync(async () =>
                {
                    var userRepo = _unitOfWork.GetRepository<User>();

                    var existing = (await userRepo
                        .SingleOrDefaultAsync(
                            predicate: u => u.UserId == id && u.IsActive == true,
                            include: q => q.Include(u => u.Role)
                        )).ValidateExists(id, "User not found for update.");

                    _mapper.Map(request, existing);
                    existing.UpdatedAt = DateTime.UtcNow;

                    userRepo.UpdateAsync(existing);
                    await _unitOfWork.CommitAsync();

                    return _mapper.Map<UserResponse>(existing);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user: {Message}", ex.Message);
                throw;
            }
        }
        
        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                return await _unitOfWork.ProcessInTransactionAsync(async () =>
                {
                    var userRepo = _unitOfWork.GetRepository<User>();

                    var existing = (await userRepo
                        .SingleOrDefaultAsync(
                            predicate: u => u.UserId == id && u.IsActive == true
                        )).ValidateExists(id, "User not found for delete.");

                    existing.IsActive = false;
                    existing.UpdatedAt = DateTime.UtcNow;

                    userRepo.UpdateAsync(existing);
                    await _unitOfWork.CommitAsync();

                    return true;
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user: {Message}", ex.Message);
                throw;
            }
        }

        public Task<IEnumerable<UserResponse>> GetTeachersAsync()
        {
            try
            {
                return _unitOfWork.ProcessInTransactionAsync(async () =>
                {
                    var users = await _unitOfWork.GetRepository<User>()
                        .GetListAsync(
                            predicate: u => u.IsActive == true && u.Role.RoleName == "Examiner",
                            include: q => q.Include(u => u.Role)
                        );

                    return _mapper.Map<IEnumerable<UserResponse>>(users);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving teachers list: {Message}", ex.Message);
                throw;
            }
        }
    } 
}