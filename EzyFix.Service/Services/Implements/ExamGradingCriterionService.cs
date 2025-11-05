//using AutoMapper;
//using EzyFix.BLL.Services.Interfaces;
//using EzyFix.DAL.Data.Exceptions;
//using EzyFix.DAL.Data.Requests.ExamGradingCriteria;
//using EzyFix.DAL.Data.Responses.ExamGradingCriteria;
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
//    public class ExamGradingCriterionService : BaseService<ExamGradingCriterionService>, IExamGradingCriterionService
//    {
//        public ExamGradingCriterionService(
//            IUnitOfWork<AppDbContext> unitOfWork,
//            ILogger<ExamGradingCriterionService> logger,
//            IMapper mapper,
//            IHttpContextAccessor httpContextAccessor)
//            : base(unitOfWork, logger, mapper, httpContextAccessor)
//        {
//        }

//        public async Task<IEnumerable<ExamGradingCriterionResponse>> GetAllExamGradingCriteriaAsync()
//        {
//            try
//            {
//                var criteria = await _unitOfWork.GetRepository<ExamGradingCriterion>().GetListAsync();
//                return _mapper.Map<IEnumerable<ExamGradingCriterionResponse>>(criteria);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "L?i khi l?y danh sách tiêu chí ch?m ?i?m: {Message}", ex.Message);
//                throw; // Ném l?i ?? middleware ho?c controller x? lý
//            }
//        }

//        public async Task<ExamGradingCriterionResponse?> GetExamGradingCriterionByIdAsync(Guid id)
//        {
//            try
//            {
//                var criterion = await _unitOfWork.GetRepository<ExamGradingCriterion>().SingleOrDefaultAsync(predicate: c => c.CriteriaId == id);

//                if (criterion == null)
//                {
//                    // Ném m?t exception c? th? ?? controller b?t và tr? v? 404
//                    throw new NotFoundException($"Không tìm th?y tiêu chí ch?m ?i?m v?i ID: {id}");
//                }

//                return _mapper.Map<ExamGradingCriterionResponse>(criterion);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "L?i khi l?y tiêu chí ch?m ?i?m theo ID {Id}: {Message}", id, ex.Message);
//                throw;
//            }
//        }

//        public async Task<ExamGradingCriterionResponse> CreateExamGradingCriterionAsync(CreateExamGradingCriterionRequest createDto)
//        {
//            try
//            {
//                return await _unitOfWork.ProcessInTransactionAsync(async () =>
//                {
//                    var criterion = _mapper.Map<ExamGradingCriterion>(createDto);

//                    // 1. T? sinh Guid m?i cho ID
//                    criterion.CriteriaId = Guid.NewGuid();

//                    await _unitOfWork.GetRepository<ExamGradingCriterion>().InsertAsync(criterion);
//                    await _unitOfWork.CommitAsync();

//                    return _mapper.Map<ExamGradingCriterionResponse>(criterion);
//                });
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "L?i khi t?o tiêu chí ch?m ?i?m m?i: {Message}", ex.Message);
//                throw;
//            }
//        }

//        public async Task<ExamGradingCriterionResponse> UpdateExamGradingCriterionAsync(Guid id, UpdateExamGradingCriterionRequest updateDto)
//        {
//            try
//            {
//                return await _unitOfWork.ProcessInTransactionAsync(async () =>
//                {
//                    // S?A: C?p nh?t predicate
//                    var criterion = await _unitOfWork.GetRepository<ExamGradingCriterion>().SingleOrDefaultAsync(predicate: c => c.CriteriaId == id);
//                    if (criterion == null)
//                    {
//                        throw new NotFoundException($"Không tìm th?y tiêu chí ch?m ?i?m v?i ID: {id} ?? c?p nh?t.");
//                    }

//                    _mapper.Map(updateDto, criterion);
//                    _unitOfWork.GetRepository<ExamGradingCriterion>().UpdateAsync(criterion); // Dùng Update ??ng b?
//                    await _unitOfWork.CommitAsync();

//                    return _mapper.Map<ExamGradingCriterionResponse>(criterion);
//                });
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "L?i khi c?p nh?t tiêu chí ch?m ?i?m {Id}: {Message}", id, ex.Message);
//                throw;
//            }
//        }

//        public async Task<bool> DeleteExamGradingCriterionAsync(Guid id)
//        {
//            try
//            {
//                return await _unitOfWork.ProcessInTransactionAsync(async () =>
//                {
//                    // S?A: C?p nh?t predicate
//                    var criterion = await _unitOfWork.GetRepository<ExamGradingCriterion>().SingleOrDefaultAsync(predicate: c => c.CriteriaId == id);
//                    if (criterion == null)
//                    {
//                        throw new NotFoundException($"Không tìm th?y tiêu chí ch?m ?i?m v?i ID: {id} ?? xóa.");
//                    }

//                    _unitOfWork.GetRepository<ExamGradingCriterion>().DeleteAsync(criterion); // Dùng Delete ??ng b?
//                    await _unitOfWork.CommitAsync();
//                    return true;
//                });
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "L?i khi xóa tiêu chí ch?m ?i?m {Id}: {Message}", id, ex.Message);
//                throw;
//            }
//        }
//    }
//}