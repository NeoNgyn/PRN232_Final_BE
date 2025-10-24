using AutoMapper;
using EzyFix.BLL.Services.Interfaces;
using EzyFix.DAL.Data.Exceptions;
using EzyFix.DAL.Data.Requests.GradingDetails;
using EzyFix.DAL.Data.Responses.GradingDetails;
using EzyFix.DAL.Models;
using EzyFix.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzyFix.BLL.Services.Implements
{
    public class GradingDetailService : BaseService<GradingDetailService>, IGradingDetailService
    {
        public GradingDetailService(
            IUnitOfWork<AppDbContext> unitOfWork,
            ILogger<GradingDetailService> logger,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<IEnumerable<GradingDetailResponse>> GetAllGradingDetailsAsync()
        {
            try
            {
                var gradingDetails = await _unitOfWork.GetRepository<GradingDetail>().GetListAsync(
                    include: q => q.Include(gd => gd.Column)
                                  .Include(gd => gd.Score)
                );
                
                var responses = gradingDetails.Select(gd =>
                {
                    var response = _mapper.Map<GradingDetailResponse>(gd);
                    response.ColumnName = gd.Column?.Name ?? "Unknown Column";
                    response.ScoreResultInfo = $"Result ID: {gd.ScoreId}";
                    return response;
                });
                
                return responses;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "L?i khi l?y danh sách chi ti?t ch?m ?i?m: {Message}", ex.Message);
                throw; // Ném l?i ?? middleware ho?c controller x? lý
            }
        }

        public async Task<GradingDetailResponse?> GetGradingDetailByIdAsync(Guid id)
        {
            try
            {
                var gradingDetail = await _unitOfWork.GetRepository<GradingDetail>().SingleOrDefaultAsync(
                    predicate: gd => gd.DetailId == id,
                    include: q => q.Include(gd => gd.Column)
                                  .Include(gd => gd.Score)
                );

                if (gradingDetail == null)
                {
                    // Ném m?t exception c? th? ?? controller b?t và tr? v? 404
                    throw new NotFoundException($"Không tìm th?y chi ti?t ch?m ?i?m v?i ID: {id}");
                }

                var response = _mapper.Map<GradingDetailResponse>(gradingDetail);
                response.ColumnName = gradingDetail.Column?.Name ?? "Unknown Column";
                response.ScoreResultInfo = $"Result ID: {gradingDetail.ScoreId}";
                
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "L?i khi l?y chi ti?t ch?m ?i?m theo ID {Id}: {Message}", id, ex.Message);
                throw;
            }
        }

        public async Task<GradingDetailResponse> CreateGradingDetailAsync(CreateGradingDetailRequest createDto)
        {
            try
            {
                // Validate that ScoreId and ColumnId exist
                await ValidateScoreAndColumnExistAsync(createDto.ScoreId, createDto.ColumnId);

                // Check for duplicate grading detail (same ScoreId and ColumnId)
                var existingDetail = await _unitOfWork.GetRepository<GradingDetail>()
                    .SingleOrDefaultAsync(predicate: gd => gd.ScoreId == createDto.ScoreId && gd.ColumnId == createDto.ColumnId);

                if (existingDetail != null)
                {
                    throw new BadRequestException($"Chi ti?t ch?m ?i?m cho c?t này ?ã t?n t?i trong k?t qu? ch?m ?i?m này.");
                }

                return await _unitOfWork.ProcessInTransactionAsync(async () =>
                {
                    var gradingDetail = _mapper.Map<GradingDetail>(createDto);

                    // 1. T? sinh Guid m?i cho ID
                    gradingDetail.DetailId = Guid.NewGuid();

                    await _unitOfWork.GetRepository<GradingDetail>().InsertAsync(gradingDetail);
                    await _unitOfWork.CommitAsync();

                    // Get the created detail with related data
                    var createdDetail = await _unitOfWork.GetRepository<GradingDetail>().SingleOrDefaultAsync(
                        predicate: gd => gd.DetailId == gradingDetail.DetailId,
                        include: q => q.Include(gd => gd.Column)
                                      .Include(gd => gd.Score)
                    );

                    var response = _mapper.Map<GradingDetailResponse>(createdDetail);
                    response.ColumnName = createdDetail.Column?.Name ?? "Unknown Column";
                    response.ScoreResultInfo = $"Result ID: {createdDetail.ScoreId}";
                    
                    return response;
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "L?i khi t?o chi ti?t ch?m ?i?m m?i: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<GradingDetailResponse> UpdateGradingDetailAsync(Guid id, UpdateGradingDetailRequest updateDto)
        {
            try
            {
                return await _unitOfWork.ProcessInTransactionAsync(async () =>
                {
                    // S?A: C?p nh?t predicate
                    var gradingDetail = await _unitOfWork.GetRepository<GradingDetail>().SingleOrDefaultAsync(
                        predicate: gd => gd.DetailId == id,
                        include: q => q.Include(gd => gd.Column)
                                      .Include(gd => gd.Score)
                    );
                    
                    if (gradingDetail == null)
                    {
                        throw new NotFoundException($"Không tìm th?y chi ti?t ch?m ?i?m v?i ID: {id} ?? c?p nh?t.");
                    }

                    // Validate new ScoreId and ColumnId if they are being changed
                    if (updateDto.ScoreId.HasValue || updateDto.ColumnId.HasValue)
                    {
                        var newScoreId = updateDto.ScoreId ?? gradingDetail.ScoreId;
                        var newColumnId = updateDto.ColumnId ?? gradingDetail.ColumnId;
                        
                        await ValidateScoreAndColumnExistAsync(newScoreId, newColumnId);

                        // Check for duplicate if combination is being changed
                        if (newScoreId != gradingDetail.ScoreId || newColumnId != gradingDetail.ColumnId)
                        {
                            var existingDetail = await _unitOfWork.GetRepository<GradingDetail>()
                                .SingleOrDefaultAsync(predicate: gd => gd.ScoreId == newScoreId && 
                                                                      gd.ColumnId == newColumnId && 
                                                                      gd.DetailId != id);

                            if (existingDetail != null)
                            {
                                throw new BadRequestException($"Chi ti?t ch?m ?i?m cho c?t này ?ã t?n t?i trong k?t qu? ch?m ?i?m này.");
                            }
                        }
                    }

                    _mapper.Map(updateDto, gradingDetail);
                    _unitOfWork.GetRepository<GradingDetail>().UpdateAsync(gradingDetail); // Dùng Update ??ng b?
                    await _unitOfWork.CommitAsync();

                    // Get updated detail with related data
                    var updatedDetail = await _unitOfWork.GetRepository<GradingDetail>().SingleOrDefaultAsync(
                        predicate: gd => gd.DetailId == id,
                        include: q => q.Include(gd => gd.Column)
                                      .Include(gd => gd.Score)
                    );

                    var response = _mapper.Map<GradingDetailResponse>(updatedDetail);
                    response.ColumnName = updatedDetail.Column?.Name ?? "Unknown Column";
                    response.ScoreResultInfo = $"Result ID: {updatedDetail.ScoreId}";
                    
                    return response;
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "L?i khi c?p nh?t chi ti?t ch?m ?i?m {Id}: {Message}", id, ex.Message);
                throw;
            }
        }

        public async Task<bool> DeleteGradingDetailAsync(Guid id)
        {
            try
            {
                return await _unitOfWork.ProcessInTransactionAsync(async () =>
                {
                    // S?A: C?p nh?t predicate
                    var gradingDetail = await _unitOfWork.GetRepository<GradingDetail>().SingleOrDefaultAsync(predicate: gd => gd.DetailId == id);
                    if (gradingDetail == null)
                    {
                        throw new NotFoundException($"Không tìm th?y chi ti?t ch?m ?i?m v?i ID: {id} ?? xóa.");
                    }

                    _unitOfWork.GetRepository<GradingDetail>().DeleteAsync(gradingDetail); // Dùng Delete ??ng b?
                    await _unitOfWork.CommitAsync();
                    return true;
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "L?i khi xóa chi ti?t ch?m ?i?m {Id}: {Message}", id, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<GradingDetailResponse>> GetGradingDetailsByScoreIdAsync(Guid scoreId)
        {
            try
            {
                var gradingDetails = await _unitOfWork.GetRepository<GradingDetail>().GetListAsync(
                    predicate: gd => gd.ScoreId == scoreId,
                    include: q => q.Include(gd => gd.Column)
                                  .Include(gd => gd.Score)
                );

                var responses = gradingDetails.Select(gd =>
                {
                    var response = _mapper.Map<GradingDetailResponse>(gd);
                    response.ColumnName = gd.Column?.Name ?? "Unknown Column";
                    response.ScoreResultInfo = $"Result ID: {gd.ScoreId}";
                    return response;
                });

                return responses;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "L?i khi l?y chi ti?t ch?m ?i?m theo ScoreId {ScoreId}: {Message}", scoreId, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<GradingDetailResponse>> GetGradingDetailsByColumnIdAsync(Guid columnId)
        {
            try
            {
                var gradingDetails = await _unitOfWork.GetRepository<GradingDetail>().GetListAsync(
                    predicate: gd => gd.ColumnId == columnId,
                    include: q => q.Include(gd => gd.Column)
                                  .Include(gd => gd.Score)
                );

                var responses = gradingDetails.Select(gd =>
                {
                    var response = _mapper.Map<GradingDetailResponse>(gd);
                    response.ColumnName = gd.Column?.Name ?? "Unknown Column";
                    response.ScoreResultInfo = $"Result ID: {gd.ScoreId}";
                    return response;
                });

                return responses;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "L?i khi l?y chi ti?t ch?m ?i?m theo ColumnId {ColumnId}: {Message}", columnId, ex.Message);
                throw;
            }
        }

        private async Task ValidateScoreAndColumnExistAsync(Guid scoreId, Guid columnId)
        {
            // Validate that the Score exists
            var scoreExists = await _unitOfWork.GetRepository<GradingResult>()
                .SingleOrDefaultAsync(predicate: gr => gr.ResultId == scoreId) != null;

            if (!scoreExists)
            {
                throw new NotFoundException($"Không tìm th?y k?t qu? ch?m ?i?m v?i ID: {scoreId}");
            }

            // Validate that the Column exists
            var columnExists = await _unitOfWork.GetRepository<ScoreColumn>()
                .SingleOrDefaultAsync(predicate: sc => sc.ColumnId == columnId) != null;

            if (!columnExists)
            {
                throw new NotFoundException($"Không tìm th?y c?t ?i?m v?i ID: {columnId}");
            }
        }
    }
}