//using AutoMapper;
//using EzyFix.BLL.Services.Interfaces;
//using EzyFix.DAL.Data.Exceptions;
//using EzyFix.DAL.Data.Requests.Exams;
//using EzyFix.DAL.Data.Responses.Exams;
//using EzyFix.DAL.Models;
//using EzyFix.DAL.Repositories.Interfaces;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace EzyFix.BLL.Services.Implements
//{
//    public class ExamService : BaseService<ExamService>, IExamService
//    {
//        public ExamService(IUnitOfWork<AppDbContext> unitOfWork, ILogger<ExamService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor)
//            : base(unitOfWork, logger, mapper, httpContextAccessor)
//        {
//        }

//        public async Task<IEnumerable<ExamResponse>> GetAllExamsAsync()
//        {
//            try
//            {
//                var exams = await _unitOfWork.GetRepository<Exam>().GetListAsync();
//                return _mapper.Map<IEnumerable<ExamResponse>>(exams);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error fetching exams: {Message}", ex.Message);
//                throw;
//            }
//        }

//        public async Task<ExamResponse?> GetExamByIdAsync(Guid id)
//        {
//            try
//            {
//                var exam = await _unitOfWork.GetRepository<Exam>()
//                    .SingleOrDefaultAsync(predicate: e => e.ExamId == id);

//                if (exam == null)
//                {
//                    throw new NotFoundException($"Exam not found with ID: {id}");
//                }

//                return _mapper.Map<ExamResponse>(exam);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error fetching exam by id {Id}: {Message}", id, ex.Message);
//                throw;
//            }
//        }

//        public async Task<ExamResponse> CreateExamAsync(CreateExamRequest createDto)
//        {
//            try
//            {
//                return await _unitOfWork.ProcessInTransactionAsync(async () =>
//                {
//                    var exam = _mapper.Map<Exam>(createDto);
//                    exam.ExamId = Guid.NewGuid();
//                    exam.CreatedAt = DateTime.UtcNow;

//                    await _unitOfWork.GetRepository<Exam>().InsertAsync(exam);
//                    await _unitOfWork.CommitAsync();

//                    return _mapper.Map<ExamResponse>(exam);
//                });
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error creating exam: {Message}", ex.Message);
//                throw;
//            }
//        }

//        public async Task<ExamResponse> UpdateExamAsync(Guid id, UpdateExamRequest updateDto)
//        {
//            try
//            {
//                return await _unitOfWork.ProcessInTransactionAsync(async () =>
//                {
//                    var exam = await _unitOfWork.GetRepository<Exam>()
//                        .SingleOrDefaultAsync(predicate: e => e.ExamId == id);

//                    if (exam == null)
//                    {
//                        throw new NotFoundException($"Exam not found with ID: {id} to update.");
//                    }

//                    _mapper.Map(updateDto, exam);
//                    exam.UpdatedAt = DateTime.UtcNow;

//                    _unitOfWork.GetRepository<Exam>().UpdateAsync(exam);
//                    await _unitOfWork.CommitAsync();

//                    return _mapper.Map<ExamResponse>(exam);
//                });
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error updating exam {Id}: {Message}", id, ex.Message);
//                throw;
//            }
//        }

//        public async Task<bool> DeleteExamAsync(Guid id)
//        {
//            try
//            {
//                return await _unitOfWork.ProcessInTransactionAsync(async () =>
//                {
//                    var exam = await _unitOfWork.GetRepository<Exam>()
//                        .SingleOrDefaultAsync(predicate: e => e.ExamId == id);

//                    if (exam == null)
//                    {
//                        throw new NotFoundException($"Exam not found with ID: {id} to delete.");
//                    }

//                    _unitOfWork.GetRepository<Exam>().DeleteAsync(exam);
//                    await _unitOfWork.CommitAsync();

//                    return true;
//                });
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error deleting exam {Id}: {Message}", id, ex.Message);
//                throw;
//            }
//        }
//    }
//}
