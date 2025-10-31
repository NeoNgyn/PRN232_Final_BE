/*
using System.Security.Cryptography;
using EzyFix.BLL.Services.Interfaces;
using EzyFix.BLL.Utils;
using EzyFix.DAL.Data.Entities;
using EzyFix.DAL.Repositories.Interfaces;

namespace EzyFix.BLL.Services.Implements
{
    public class RefreshTokensService : IRefreshTokensService
    {
        private readonly IUnitOfWork<EzyFixDbContext> _unitOfWork;
        private readonly IJwtUtil _jwtUtil;

        public RefreshTokensService(IUnitOfWork<EzyFixDbContext> unitOfWork, IJwtUtil jwtUtil)
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

                var staff = await _unitOfWork.GetRepository<Staff>().GetByIdAsync(storedToken.UserId);
                if (staff == null) throw new Exception("Staff not found");
                Tuple<string, Guid> guidSecurityClaim = new Tuple<string, Guid>("StaffId", staff.Id);

                return _jwtUtil.GenerateJwtToken(staff, guidSecurityClaim, false);
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
*/
using EzyFix.BLL.Services.Interfaces;
using EzyFix.BLL.Utils;
using EzyFix.DAL.Models; // <-- Giả sử namespace này chứa 'RefreshTokens'
// Using cho các model của bạn// <-- Namespace chứa 'User'
using EzyFix.DAL.Repositories.Interfaces;
using System.Security.Cryptography;

namespace EzyFix.BLL.Services.Implements
{
    public class RefreshTokensService : IRefreshTokensService
    {
        private readonly IUnitOfWork<AppDbContext> _unitOfWork;
        private readonly IJwtUtil _jwtUtil;

        public RefreshTokensService(IUnitOfWork<AppDbContext> unitOfWork, IJwtUtil jwtUtil)
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

                // === ĐÃ SỬA TỪ ĐÂY ===

                // 1. Đổi GetRepository<Staff> thành GetRepository<User>
                var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(storedToken.UserId);

                // 2. Sửa lại thông báo lỗi
                if (user == null) throw new Exception("User not found");

                // 3. Sửa lại claim (Giả sử model 'User' của bạn có 'UserId')
                Tuple<string, Guid> guidSecurityClaim = new Tuple<string, Guid>("UserId", user.UserId);

                // 4. Truyền 'user' (kiểu User) vào JwtUtil, giờ đã khớp
                return _jwtUtil.GenerateJwtToken(user, guidSecurityClaim, false);

                // === SỬA ĐẾN ĐÂY ===
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