//using AutoMapper;
//using EzyFix.BLL.Services.Interfaces;
//using EzyFix.DAL.Data.Exceptions;
//using EzyFix.DAL.Data.MetaDatas;
//using EzyFix.DAL.Data.Requests.Keywords;
//using EzyFix.DAL.Data.Responses.Keywords;
//using EzyFix.DAL.Models;
//using EzyFix.DAL.Repositories.Interfaces;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace EzyFix.BLL.Services.Implements
//{
//    public class KeywordService : BaseService<KeywordService>, IKeywordService
//    {
//        public KeywordService(
//            IUnitOfWork<AppDbContext> unitOfWork,
//            ILogger<KeywordService> logger,
//            IMapper mapper,
//            IHttpContextAccessor httpContextAccessor)
//            : base(unitOfWork, logger, mapper, httpContextAccessor)
//        {
//        }

//        public async Task<IEnumerable<KeywordResponse>> GetAllKeywordsAsync()
//        {
//            try
//            {
//                var keywords = await _unitOfWork.GetRepository<Keyword>().GetListAsync();
//                return _mapper.Map<IEnumerable<KeywordResponse>>(keywords);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Lỗi khi lấy danh sách từ khóa: {Message}", ex.Message);
//                throw;
//            }
//        }

//        public async Task<KeywordResponse?> GetKeywordByIdAsync(Guid id)
//        {
//            try
//            {
//                var keyword = await _unitOfWork.GetRepository<Keyword>()
//                    .SingleOrDefaultAsync(predicate: k => k.KeywordId == id); // SỬA: Cập nhật predicate

//                if (keyword == null)
//                {
//                    throw new NotFoundException($"Không tìm thấy từ khóa với ID: {id}");
//                }
//                return _mapper.Map<KeywordResponse>(keyword);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Lỗi khi lấy từ khóa theo ID {Id}: {Message}", id, ex.Message);
//                throw;
//            }
//        }

//        public async Task<KeywordResponse> CreateKeywordAsync(CreateKeywordRequest createDto)
//        {
//            try
//            {
//                // Kiểm tra từ khóa trùng lặp vẫn hợp lệ
//                var existing = await _unitOfWork.GetRepository<Keyword>()
//                    .SingleOrDefaultAsync(predicate: k => k.Word.ToLower() == createDto.Word.ToLower());

//                if (existing != null)
//                {
//                    throw new BadRequestException($"Từ khóa '{createDto.Word}' đã tồn tại.");
//                }

//                return await _unitOfWork.ProcessInTransactionAsync(async () =>
//                {
//                    var keyword = _mapper.Map<Keyword>(createDto);

//                    // SỬA: Tự sinh Guid mới cho ID
//                    keyword.KeywordId = Guid.NewGuid();

//                    await _unitOfWork.GetRepository<Keyword>().InsertAsync(keyword);
//                    await _unitOfWork.CommitAsync();

//                    return _mapper.Map<KeywordResponse>(keyword);
//                });
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Lỗi khi tạo từ khóa mới: {Message}", ex.Message);
//                throw;
//            }
//        }

//        public async Task<KeywordResponse> UpdateKeywordAsync(Guid id, UpdateKeywordRequest updateDto)
//        {
//            try
//            {
//                return await _unitOfWork.ProcessInTransactionAsync(async () =>
//                {
//                    var keyword = await _unitOfWork.GetRepository<Keyword>()
//                        .SingleOrDefaultAsync(predicate: k => k.KeywordId == id); // SỬA: Cập nhật predicate

//                    if (keyword == null)
//                    {
//                        throw new NotFoundException($"Không tìm thấy từ khóa với ID: {id} để cập nhật.");
//                    }

//                    _mapper.Map(updateDto, keyword);
//                    _unitOfWork.GetRepository<Keyword>().UpdateAsync(keyword);
//                    await _unitOfWork.CommitAsync();

//                    return _mapper.Map<KeywordResponse>(keyword);
//                });
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Lỗi khi cập nhật từ khóa {Id}: {Message}", id, ex.Message);
//                throw;
//            }
//        }

//        public async Task<bool> DeleteKeywordAsync(Guid id)
//        {
//            try
//            {
//                return await _unitOfWork.ProcessInTransactionAsync(async () =>
//                {
//                    var keyword = await _unitOfWork.GetRepository<Keyword>()
//                        .SingleOrDefaultAsync(predicate: k => k.KeywordId == id); // SỬA: Cập nhật predicate

//                    if (keyword == null)
//                    {
//                        throw new NotFoundException($"Không tìm thấy từ khóa với ID: {id} để xóa.");
//                    }

//                    _unitOfWork.GetRepository<Keyword>().DeleteAsync(keyword);
//                    await _unitOfWork.CommitAsync();
//                    return true;
//                });
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Lỗi khi xóa từ khóa {Id}: {Message}", id, ex.Message);
//                throw;
//            }
//        }
//    }
//}
