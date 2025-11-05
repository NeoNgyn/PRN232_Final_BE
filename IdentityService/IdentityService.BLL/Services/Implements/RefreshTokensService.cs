using IdentityService.BLL.Services.Interfaces;
using IdentityService.BLL.Utils;
using IdentityService.DAL.Models;
using IdentityService.DAL.Repositories.Interfaces;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.BLL.Services.Implements
{
    public class RefreshTokensService : IRefreshTokensService
    {
        private readonly IUnitOfWork<IdentityDbContext> _unitOfWork;
        private readonly IJwtUtil _jwtUtil;

        public RefreshTokensService(IUnitOfWork<IdentityDbContext> unitOfWork, IJwtUtil jwtUtil)
        {
            _unitOfWork = unitOfWork;
            _jwtUtil = jwtUtil;
        }

        public async Task<string> GenerateAndStoreRefreshToken(Guid userId)
        {
            return await _unitOfWork.ProcessInTransactionAsync(async () =>
            {
                var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
                var refreshTokenRepo = _unitOfWork.GetRepository<RefreshTokens>();
                var newToken = new RefreshTokens
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Token = refreshToken,
                    CreateAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddDays(7)
                };

                await refreshTokenRepo.InsertAsync(newToken);
                return refreshToken;
            });
        }

        public async Task<string> RefreshAccessToken(string refreshToken)
        {
            return await _unitOfWork.ProcessInTransactionAsync(async () =>
            {
                var refreshTokenRepo = _unitOfWork.GetRepository<RefreshTokens>();
                var storedToken = await refreshTokenRepo.SingleOrDefaultAsync<RefreshTokens>(t => t, t => t.Token == refreshToken);

                if (storedToken == null || storedToken.ExpiresAt < DateTime.UtcNow)
                {
                    throw new Exception("Invalid or expired refresh token");
                }

                var user = await _unitOfWork.GetRepository<User>().SingleOrDefaultAsync(
                    predicate: u => u.UserId == storedToken.UserId,
                    include: q => q.Include(u => u.Role)
                );

                if (user == null)
                    throw new Exception("User not found");

                string roleName = user.Role?.RoleName ?? "Student";
                Tuple<string, Guid> guidSecurityClaim = new Tuple<string, Guid>("UserId", user.UserId);

                return _jwtUtil.GenerateJwtToken(user, guidSecurityClaim, roleName, false);
            });
        }

        public async Task<bool> DeleteRefreshToken(string refreshToken)
        {
            return await _unitOfWork.ProcessInTransactionAsync(async () =>
            {
                var refreshTokenRepo = _unitOfWork.GetRepository<RefreshTokens>();
                var tokenToDelete = await refreshTokenRepo.SingleOrDefaultAsync(
                    predicate: t => t.Token == refreshToken);

                if (tokenToDelete != null)
                {
                    refreshTokenRepo.DeleteAsync(tokenToDelete);
                    return true;
                }
                return false;
            });
        }
    }
}
