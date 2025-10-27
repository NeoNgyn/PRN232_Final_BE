using AutoMapper;
using EzyFix.BLL.Services.Interfaces;
using EzyFix.DAL.Data.Exceptions;
using EzyFix.DAL.Data.Requests.ExamKeyword;
using EzyFix.DAL.Data.Responses.ExamKeyword;
using EzyFix.DAL.Models;
using EzyFix.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EzyFix.BLL.Services.Implements
{
    public class ExamKeywordService : BaseService<ExamKeywordService>, IExamKeywordService
    {
        public ExamKeywordService(
            IUnitOfWork<AppDbContext> unitOfWork,
            ILogger<ExamKeywordService> logger,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<IEnumerable<ExamKeywordResponse>> GetAllExamKeywordsAsync()
        {
            try
            {
                var examKeywords = await _unitOfWork.GetRepository<ExamKeyword>()
                    .GetListAsync(
                        include: q => q.Include(ek => ek.Exam)
                                       .Include(ek => ek.Keyword)
                    );

                return _mapper.Map<IEnumerable<ExamKeywordResponse>>(examKeywords);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving exam keyword list: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<ExamKeywordResponse?> GetExamKeywordByIdAsync(Guid id)
        {
            try
            {
                var examKeyword = await _unitOfWork.GetRepository<ExamKeyword>()
                    .SingleOrDefaultAsync(
                        predicate: ek => ek.ExamKeywordId == id,
                        include: q => q.Include(ek => ek.Exam)
                                       .Include(ek => ek.Keyword)
                    );

                if (examKeyword == null)
                {
                    throw new NotFoundException($"ExamKeyword with ID {id} not found");
                }

                return _mapper.Map<ExamKeywordResponse>(examKeyword);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving exam keyword by ID {Id}: {Message}", id, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<ExamKeywordResponse>> GetExamKeywordsByExamIdAsync(Guid examId)
        {
            try
            {
                var examKeywords = await _unitOfWork.GetRepository<ExamKeyword>()
                    .GetListAsync(
                        predicate: ek => ek.ExamId == examId,
                        include: q => q.Include(ek => ek.Exam)
                                       .Include(ek => ek.Keyword)
                    );

                return _mapper.Map<IEnumerable<ExamKeywordResponse>>(examKeywords);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving exam keywords by exam ID {ExamId}: {Message}", examId, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<ExamKeywordResponse>> GetExamKeywordsByKeywordIdAsync(Guid keywordId)
        {
            try
            {
                var examKeywords = await _unitOfWork.GetRepository<ExamKeyword>()
                    .GetListAsync(
                        predicate: ek => ek.KeywordId == keywordId,
                        include: q => q.Include(ek => ek.Exam)
                                       .Include(ek => ek.Keyword)
                    );

                return _mapper.Map<IEnumerable<ExamKeywordResponse>>(examKeywords);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving exam keywords by keyword ID {KeywordId}: {Message}", keywordId, ex.Message);
                throw;
            }
        }

        public async Task<ExamKeywordResponse> CreateExamKeywordAsync(CreateExamKeywordRequest createDto)
        {
            try
            {
                return await _unitOfWork.ProcessInTransactionAsync(async () =>
                {
                    // Check if exam exists
                    var exam = await _unitOfWork.GetRepository<Exam>()
                        .SingleOrDefaultAsync(predicate: e => e.ExamId == createDto.ExamId);
                    
                    if (exam == null)
                    {
                        throw new NotFoundException($"Exam with ID {createDto.ExamId} not found");
                    }

                    // Check if keyword exists
                    var keyword = await _unitOfWork.GetRepository<Keyword>()
                        .SingleOrDefaultAsync(predicate: k => k.KeywordId == createDto.KeywordId);
                    
                    if (keyword == null)
                    {
                        throw new NotFoundException($"Keyword with ID {createDto.KeywordId} not found");
                    }

                    // Check for duplicate
                    var existing = await _unitOfWork.GetRepository<ExamKeyword>()
                        .SingleOrDefaultAsync(predicate: ek => 
                            ek.ExamId == createDto.ExamId && 
                            ek.KeywordId == createDto.KeywordId);

                    if (existing != null)
                    {
                        throw new BadRequestException($"This exam is already associated with this keyword");
                    }

                    var examKeyword = _mapper.Map<ExamKeyword>(createDto);
                    examKeyword.ExamKeywordId = Guid.NewGuid();

                    await _unitOfWork.GetRepository<ExamKeyword>().InsertAsync(examKeyword);
                    await _unitOfWork.CommitAsync();

                    // Reload with related entities
                    var created = await _unitOfWork.GetRepository<ExamKeyword>()
                        .SingleOrDefaultAsync(
                            predicate: ek => ek.ExamKeywordId == examKeyword.ExamKeywordId,
                            include: q => q.Include(ek => ek.Exam)
                                           .Include(ek => ek.Keyword)
                        );

                    return _mapper.Map<ExamKeywordResponse>(created);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating exam keyword: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<ExamKeywordResponse> UpdateExamKeywordAsync(Guid id, UpdateExamKeywordRequest updateDto)
        {
            try
            {
                return await _unitOfWork.ProcessInTransactionAsync(async () =>
                {
                    var examKeyword = await _unitOfWork.GetRepository<ExamKeyword>()
                        .SingleOrDefaultAsync(predicate: ek => ek.ExamKeywordId == id);

                    if (examKeyword == null)
                    {
                        throw new NotFoundException($"ExamKeyword with ID {id} not found for update");
                    }

                    // Check if new exam exists
                    var exam = await _unitOfWork.GetRepository<Exam>()
                        .SingleOrDefaultAsync(predicate: e => e.ExamId == updateDto.ExamId);
                    
                    if (exam == null)
                    {
                        throw new NotFoundException($"Exam with ID {updateDto.ExamId} not found");
                    }

                    // Check if new keyword exists
                    var keyword = await _unitOfWork.GetRepository<Keyword>()
                        .SingleOrDefaultAsync(predicate: k => k.KeywordId == updateDto.KeywordId);
                    
                    if (keyword == null)
                    {
                        throw new NotFoundException($"Keyword with ID {updateDto.KeywordId} not found");
                    }

                    // Check for duplicate (excluding current record)
                    var existing = await _unitOfWork.GetRepository<ExamKeyword>()
                        .SingleOrDefaultAsync(predicate: ek => 
                            ek.ExamId == updateDto.ExamId && 
                            ek.KeywordId == updateDto.KeywordId &&
                            ek.ExamKeywordId != id);

                    if (existing != null)
                    {
                        throw new BadRequestException($"This exam is already associated with this keyword");
                    }

                    _mapper.Map(updateDto, examKeyword);
                    _unitOfWork.GetRepository<ExamKeyword>().UpdateAsync(examKeyword);
                    await _unitOfWork.CommitAsync();

                    // Reload with related entities
                    var updated = await _unitOfWork.GetRepository<ExamKeyword>()
                        .SingleOrDefaultAsync(
                            predicate: ek => ek.ExamKeywordId == id,
                            include: q => q.Include(ek => ek.Exam)
                                           .Include(ek => ek.Keyword)
                        );

                    return _mapper.Map<ExamKeywordResponse>(updated);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating exam keyword {Id}: {Message}", id, ex.Message);
                throw;
            }
        }

        public async Task<bool> DeleteExamKeywordAsync(Guid id)
        {
            try
            {
                return await _unitOfWork.ProcessInTransactionAsync(async () =>
                {
                    var examKeyword = await _unitOfWork.GetRepository<ExamKeyword>()
                        .SingleOrDefaultAsync(predicate: ek => ek.ExamKeywordId == id);

                    if (examKeyword == null)
                    {
                        throw new NotFoundException($"ExamKeyword with ID {id} not found for deletion");
                    }

                    _unitOfWork.GetRepository<ExamKeyword>().DeleteAsync(examKeyword);
                    await _unitOfWork.CommitAsync();
                    return true;
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting exam keyword {Id}: {Message}", id, ex.Message);
                throw;
            }
        }

        public async Task<bool> DeleteExamKeywordsByExamIdAsync(Guid examId)
        {
            try
            {
                return await _unitOfWork.ProcessInTransactionAsync(async () =>
                {
                    var examKeywords = await _unitOfWork.GetRepository<ExamKeyword>()
                        .GetListAsync(predicate: ek => ek.ExamId == examId);

                    if (!examKeywords.Any())
                    {
                        throw new NotFoundException($"No exam keywords found for exam ID {examId}");
                    }

                    foreach (var examKeyword in examKeywords)
                    {
                        _unitOfWork.GetRepository<ExamKeyword>().DeleteAsync(examKeyword);
                    }

                    await _unitOfWork.CommitAsync();
                    return true;
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting exam keywords for exam {ExamId}: {Message}", examId, ex.Message);
                throw;
            }
        }
    }
}
