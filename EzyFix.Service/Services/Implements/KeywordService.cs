using AutoMapper;
using EzyFix.BLL.Services.Interfaces;
using EzyFix.DAL.Data.Exceptions;
using EzyFix.DAL.Data.MetaDatas;
using EzyFix.DAL.Data.Requests.Keywords;
using EzyFix.DAL.Data.Responses.Keywords;
using EzyFix.DAL.Models;
using EzyFix.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzyFix.BLL.Services.Implements
{
    public class KeywordService : BaseService<KeywordService>, IKeywordService
    {
        public KeywordService(
            IUnitOfWork<AppDbContext> unitOfWork,
            ILogger<KeywordService> logger,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<IEnumerable<KeywordResponseDto>> GetAllKeywordsAsync()
        {
            try
            {
                var keywords = await _unitOfWork.GetRepository<Keyword>().GetListAsync();
                return _mapper.Map<IEnumerable<KeywordResponseDto>>(keywords);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách từ khóa: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<KeywordResponseDto?> GetKeywordByIdAsync(int id)
        {
            try
            {
                var keyword = await _unitOfWork.GetRepository<Keyword>()
                    .SingleOrDefaultAsync(predicate: k => k.KeywordId == id);

                if (keyword == null)
                {
                    throw new NotFoundException($"Không tìm thấy từ khóa với ID: {id}");
                }

                return _mapper.Map<KeywordResponseDto>(keyword);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy từ khóa theo ID {Id}: {Message}", id, ex.Message);
                throw;
            }
        }

        public async Task<KeywordResponseDto> CreateKeywordAsync(CreateKeywordRequestDto createDto)
        {
            try
            {
                var existing = await _unitOfWork.GetRepository<Keyword>()
                    .SingleOrDefaultAsync(predicate: k => k.Word == createDto.Word);

                if (existing != null)
                {
                    throw new BadRequestException($"Từ khóa '{createDto.Word}' đã tồn tại.");
                }

                return await _unitOfWork.ProcessInTransactionAsync(async () =>
                {
                    var keyword = _mapper.Map<Keyword>(createDto);
                    await _unitOfWork.GetRepository<Keyword>().InsertAsync(keyword);
                    await _unitOfWork.CommitAsync();
                    return _mapper.Map<KeywordResponseDto>(keyword);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo từ khóa mới: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<KeywordResponseDto> UpdateKeywordAsync(int id, UpdateKeywordRequestDto updateDto)
        {
            try
            {
                return await _unitOfWork.ProcessInTransactionAsync(async () =>
                {
                    var keyword = await _unitOfWork.GetRepository<Keyword>()
                        .SingleOrDefaultAsync(predicate: k => k.KeywordId == id);

                    if (keyword == null)
                    {
                        throw new NotFoundException($"Không tìm thấy từ khóa với ID: {id} để cập nhật.");
                    }

                    _mapper.Map(updateDto, keyword);
                    _unitOfWork.GetRepository<Keyword>().UpdateAsync(keyword);
                    await _unitOfWork.CommitAsync();

                    return _mapper.Map<KeywordResponseDto>(keyword);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật từ khóa {Id}: {Message}", id, ex.Message);
                throw;
            }
        }

        public async Task<bool> DeleteKeywordAsync(int id)
        {
            try
            {
                return await _unitOfWork.ProcessInTransactionAsync(async () =>
                {
                    var keyword = await _unitOfWork.GetRepository<Keyword>()
                        .SingleOrDefaultAsync(predicate: k => k.KeywordId == id);

                    if (keyword == null)
                    {
                        throw new NotFoundException($"Không tìm thấy từ khóa với ID: {id} để xóa.");
                    }

                    _unitOfWork.GetRepository<Keyword>().DeleteAsync(keyword);
                    await _unitOfWork.CommitAsync();
                    return true;
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa từ khóa {Id}: {Message}", id, ex.Message);
                throw;
            }
        }
    }
}
