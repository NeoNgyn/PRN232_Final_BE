using AutoMapper;
using EzyFix.BLL.Services.Interfaces;
using EzyFix.DAL.Data.Exceptions;
using EzyFix.DAL.Data.Requests.ScoreColumns;
using EzyFix.DAL.Data.Responses.ScoreColumns;
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
    public class ScoreColumnService : BaseService<ScoreColumnService>, IScoreColumnService
    {
        public ScoreColumnService(IUnitOfWork<AppDbContext> unitOfWork, ILogger<ScoreColumnService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor)
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<IEnumerable<ScoreColumnResponse>> GetAllScoreColumnsAsync()
        {
            try
            {
                var scoreColumns = await _unitOfWork.GetRepository<ScoreColumn>().GetListAsync();
                return _mapper.Map<IEnumerable<ScoreColumnResponse>>(scoreColumns);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách cột điểm: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<ScoreColumnResponse?> GetScoreColumnByIdAsync(Guid id)
        {
            try
            {
                var scoreColumn = await _unitOfWork.GetRepository<ScoreColumn>()
                    .SingleOrDefaultAsync(predicate: sc => sc.ColumnId == id);

                if (scoreColumn == null)
                {
                    throw new NotFoundException($"Không tìm thấy cột điểm với ID: {id}");
                }
                return _mapper.Map<ScoreColumnResponse>(scoreColumn);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy cột điểm theo ID {Id}: {Message}", id, ex.Message);
                throw;
            }
        }

        public async Task<ScoreColumnResponse> CreateScoreColumnAsync(CreateScoreColumnRequest createDto)
        {
            try
            {
                return await _unitOfWork.ProcessInTransactionAsync(async () =>
                {
                    var scoreColumn = _mapper.Map<ScoreColumn>(createDto);

                    // Tự sinh Guid mới cho ID
                    scoreColumn.ColumnId = Guid.NewGuid();

                    await _unitOfWork.GetRepository<ScoreColumn>().InsertAsync(scoreColumn);
                    await _unitOfWork.CommitAsync();

                    return _mapper.Map<ScoreColumnResponse>(scoreColumn);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo cột điểm mới: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<ScoreColumnResponse> UpdateScoreColumnAsync(Guid id, UpdateScoreColumnRequest updateDto)
        {
            try
            {
                return await _unitOfWork.ProcessInTransactionAsync(async () =>
                {
                    var scoreColumn = await _unitOfWork.GetRepository<ScoreColumn>()
                        .SingleOrDefaultAsync(predicate: sc => sc.ColumnId == id);

                    if (scoreColumn == null)
                    {
                        throw new NotFoundException($"Không tìm thấy cột điểm với ID: {id} để cập nhật.");
                    }

                    _mapper.Map(updateDto, scoreColumn);
                    _unitOfWork.GetRepository<ScoreColumn>().UpdateAsync(scoreColumn);
                    await _unitOfWork.CommitAsync();

                    return _mapper.Map<ScoreColumnResponse>(scoreColumn);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật cột điểm {Id}: {Message}", id, ex.Message);
                throw;
            }
        }

        public async Task<bool> DeleteScoreColumnAsync(Guid id)
        {
            try
            {
                return await _unitOfWork.ProcessInTransactionAsync(async () =>
                {
                    var scoreColumn = await _unitOfWork.GetRepository<ScoreColumn>()
                        .SingleOrDefaultAsync(predicate: sc => sc.ColumnId == id);

                    if (scoreColumn == null)
                    {
                        throw new NotFoundException($"Không tìm thấy cột điểm với ID: {id} để xóa.");
                    }

                    _unitOfWork.GetRepository<ScoreColumn>().DeleteAsync(scoreColumn);
                    await _unitOfWork.CommitAsync();
                    return true;
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa cột điểm {Id}: {Message}", id, ex.Message);
                throw;
            }
        }
    }
}

