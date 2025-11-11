using AcademicService.BLL.Extension;
using AcademicService.BLL.Services.Interfaces;
using AcademicService.DAL.Data.Requests.Criteria;
using AcademicService.DAL.Data.Requests.Submission;
using AcademicService.DAL.Data.Responses.Criteria;
using AcademicService.DAL.Data.Responses.Submission;
using AcademicService.DAL.Models;
using AcademicService.DAL.Repositories.Interfaces;
using AutoMapper;
using EzyFix.BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicService.BLL.Services.Implements
{
    public class CriteriaService : BaseService<CriteriaService>, ICritieriaService
    {
        public CriteriaService(
        IUnitOfWork<AcademicDbContext> unitOfWork,
        ILogger<CriteriaService> logger,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor,
        IConfiguration configuration
    ) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {

        }

        public async Task<CriteriaListResponse> CreateCriteriaAsync(CreateCriteriaRequest request)
        {
            try
            {
                if (request == null)
                {
                    throw new ArgumentNullException(nameof(request), "Request data cannot be null.");
                }

                return await _unitOfWork.ProcessInTransactionAsync(async () =>
                {
                    var newCriteria = _mapper.Map<Criteria>(request);
                    newCriteria.CriteriaId = Guid.NewGuid();

                    await _unitOfWork.GetRepository<Criteria>().InsertAsync(newCriteria);
                    await _unitOfWork.CommitAsync();

                    return _mapper.Map<CriteriaListResponse>(newCriteria);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating criteria: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<bool> DeleteCriteriaAsync(Guid id)
        {
            try
            {
                return await _unitOfWork.ProcessInTransactionAsync(async () =>
                {
                    var criteria = (await _unitOfWork.GetRepository<Criteria>()
                        .SingleOrDefaultAsync(
                            predicate: s => s.CriteriaId == id
                        )).ValidateExists(id, "Can not find this Criteria because it isn't existed");

                    _unitOfWork.GetRepository<Criteria>().DeleteAsync(criteria);

                    await _unitOfWork.CommitAsync();

                    return true;
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting criteria: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<CriteriaListResponse>> GetAllCriteriasAsync(CriteriaQueryParameter queryParameter)
        {
            try
            {
                var criterias = await _unitOfWork.GetRepository<Criteria>()
                    .GetListAsync(
                        predicate: c => c.ExamId == queryParameter.ExamId
                    );

                return _mapper.Map<IEnumerable<CriteriaListResponse>>(criterias);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving criteria list: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<CriteriaListResponse> GetCriteriaByIdAsync(Guid id)
        {
            try
            {
                var criteria = (await _unitOfWork.GetRepository<Criteria>()
                    .SingleOrDefaultAsync(
                        predicate: c => c.CriteriaId == id
                    )).ValidateExists(id, "Can not find this Criteria because it isn't existed");

                return _mapper.Map<CriteriaListResponse>(criteria);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving criteria {Message}", ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<CriteriaListResponse>> GetQueryCriteriasAsync()
        {
            try
            {
                var criterias = _unitOfWork.GetRepository<Criteria>()
                    .GetQueryable();

                return _mapper.Map<IEnumerable<CriteriaListResponse>>(criterias);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving criteria list: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<CriteriaListResponse> UpdateCriteriaAsync(Guid id, UpdateCriteriaRequest request)
        {
            try
            {
                if (request == null)
                {
                    throw new ArgumentNullException(nameof(request), "Request data cannot be null.");
                }

                return await _unitOfWork.ProcessInTransactionAsync(async () =>
                {
                    var criteria = (await _unitOfWork.GetRepository<Criteria>()
                        .SingleOrDefaultAsync(
                            predicate: s => s.CriteriaId == id
                        )).ValidateExists(id, "Can't update because this Criteria isn't existed!");


                    _mapper.Map(request, criteria);


                    _unitOfWork.GetRepository<Criteria>().UpdateAsync(criteria);
                    await _unitOfWork.CommitAsync();

                    return _mapper.Map<CriteriaListResponse>(criteria);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating criteria: {Message}", ex.Message);
                throw;
            }
        }
    }
}
