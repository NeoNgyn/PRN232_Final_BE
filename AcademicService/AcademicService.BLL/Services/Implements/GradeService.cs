using AcademicService.BLL.Extension;
using AcademicService.BLL.Services.Interfaces;
using AcademicService.DAL.Data.Requests.Grade;
using AcademicService.DAL.Data.Responses.Criteria;
using AcademicService.DAL.Data.Responses.Grade;
using AcademicService.DAL.Models;
using AcademicService.DAL.Repositories.Interfaces;
using AutoMapper;
using EzyFix.BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicService.BLL.Services.Implements
{
    public class GradeService : BaseService<GradeService>, IGradeService
    {
        public GradeService(
        IUnitOfWork<AcademicDbContext> unitOfWork,
        ILogger<GradeService> logger,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor,
        IConfiguration configuration
    ) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {

        }

        public async Task<GradeListResponse> CreateGradeAsync(CreateGradeRequest request)
        {
            try
            {
                if (request == null)
                {
                    throw new ArgumentNullException(nameof(request), "Request data cannot be null.");
                }

                return await _unitOfWork.ProcessInTransactionAsync(async () =>
                {
                    var newGrade = _mapper.Map<Grade>(request);
                    var criteria = await _unitOfWork.GetRepository<Criteria>()
                        .SingleOrDefaultAsync(
                            predicate: c => c.CriteriaId == request.CriteriaId
                        );

                    newGrade.GradeId = Guid.NewGuid();

                    if (criteria != null && request.Score > criteria.MaxScore)
                    {
                        throw new ArgumentException($"Score cannot exceed the maximum score of {criteria.MaxScore} for the specified criteria.");
                    }

                    await _unitOfWork.GetRepository<Grade>().InsertAsync(newGrade);
                    await _unitOfWork.CommitAsync();

                    return _mapper.Map<GradeListResponse>(newGrade);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating grade: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<bool> DeleteGradeAsync(Guid id)
        {
            try
            {
                return await _unitOfWork.ProcessInTransactionAsync(async () =>
                {
                    var grade = (await _unitOfWork.GetRepository<Grade>()
                        .SingleOrDefaultAsync(
                            predicate: s => s.GradeId == id
                        )).ValidateExists(id, "Can not find this Criteria because it isn't existed");

                    grade.Score = 0;

                    _unitOfWork.GetRepository<Grade>().UpdateAsync(grade);
                    await _unitOfWork.CommitAsync();

                    return true;
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting grade: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<GradeListResponse>> GetAllGradesAsync()
        {
            try
            {
                var grades = await _unitOfWork.GetRepository<Grade>()
                    .GetListAsync();

                return _mapper.Map<IEnumerable<GradeListResponse>>(grades);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving grade list: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<GradeListResponse> GetGradeByIdAsync(Guid id)
        {
            try
            {
                var grade = (await _unitOfWork.GetRepository<Grade>()
                    .SingleOrDefaultAsync(
                        predicate: c => c.GradeId == id
                    )).ValidateExists(id, "Can not find this Grade because it isn't existed");

                return _mapper.Map<GradeListResponse>(grade);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving grade {Message}", ex.Message);
                throw;
            }
        }

        public async Task<GradeListResponse> UpdateGradeAsync(Guid id, UpdateGradeRequest request)
        {
            try
            {
                if (request == null)
                {
                    throw new ArgumentNullException(nameof(request), "Request data cannot be null.");
                }

                return await _unitOfWork.ProcessInTransactionAsync(async () =>
                {
                    var grade = (await _unitOfWork.GetRepository<Grade>()
                        .SingleOrDefaultAsync(
                            predicate: s => s.GradeId == id
                        )).ValidateExists(id, "Can't update because this Grade isn't existed!");
                    var criteria = await _unitOfWork.GetRepository<Criteria>().SingleOrDefaultAsync(
                        predicate: c => c.CriteriaId == grade.CriteriaId
                    );

                    if (criteria != null && request.Score > criteria.MaxScore)
                    {
                        throw new ArgumentException($"Score cannot exceed the maximum score of {criteria.MaxScore} for the specified criteria.");
                    }

                    _mapper.Map(request, grade);


                    _unitOfWork.GetRepository<Grade>().UpdateAsync(grade);
                    await _unitOfWork.CommitAsync();

                    return _mapper.Map<GradeListResponse>(grade);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating grade: {Message}", ex.Message);
                throw;
            }
        }
    }
}
