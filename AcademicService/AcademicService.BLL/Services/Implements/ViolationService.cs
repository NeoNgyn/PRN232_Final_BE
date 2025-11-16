using AcademicService.BLL.Extension;
using AcademicService.BLL.Services.Interfaces;
using AcademicService.DAL.Data.Requests.Violation;
using AcademicService.DAL.Data.Responses.Grade;
using AcademicService.DAL.Data.Responses.Violation;
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
    public class ViolationService : BaseService<ViolationService>, IViolationService
    {
        public ViolationService(
        IUnitOfWork<AcademicDbContext> unitOfWork,
        ILogger<ViolationService> logger,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor,
        IConfiguration configuration
    ) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {

        }

        public async Task<ViolationListResponse> CreateViolationAsync(CreateViolationRequest request)
        {
            try
            {
                if (request == null)
                {
                    throw new ArgumentNullException(nameof(request), "Request data cannot be null.");
                }

                return await _unitOfWork.ProcessInTransactionAsync(async () =>
                {
                    var newViolation = _mapper.Map<Violation>(request);

                    newViolation.ViolationId = Guid.NewGuid();
                    newViolation.DetectedAt = DateTime.UtcNow;

                    await _unitOfWork.GetRepository<Violation>().InsertAsync(newViolation);
                    await _unitOfWork.CommitAsync();

                    return _mapper.Map<ViolationListResponse>(newViolation);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating violation: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<bool> DeleteViolationAsync(Guid id)
        {
            try
            {
                return await _unitOfWork.ProcessInTransactionAsync(async () =>
                {
                    var violation = (await _unitOfWork.GetRepository<Violation>()
                        .SingleOrDefaultAsync(
                            predicate: s => s.ViolationId == id
                        )).ValidateExists(id, "Can not find this Violation because it isn't existed");


                    _unitOfWork.GetRepository<Violation>().DeleteAsync(violation);
                    await _unitOfWork.CommitAsync();

                    return true;
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting violation: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<ViolationListResponse>> GetAllViolationsAsync()
        {
            try
            {
                var violations = await _unitOfWork.GetRepository<Violation>()
                    .GetListAsync();

                return _mapper.Map<IEnumerable<ViolationListResponse>>(violations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving violation list: {Message}", ex.Message);
                throw;
            }
        }

        public Task<IEnumerable<ViolationListResponse>> GetQueryViolationsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ViolationListResponse> GetViolationByIdAsync(Guid id)
        {
            try
            {
                var violation = (await _unitOfWork.GetRepository<Violation>()
                    .SingleOrDefaultAsync(
                        predicate: c => c.ViolationId == id
                    )).ValidateExists(id, "Can not find this Violation because it isn't existed");

                return _mapper.Map<ViolationListResponse>(violation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving violation {Message}", ex.Message);
                throw;
            }
        }

        public async Task<ViolationListResponse> UpdateViolationAsync(Guid id, UpdateViolationRequest request)
        {
            try
            {
                if (request == null)
                {
                    throw new ArgumentNullException(nameof(request), "Request data cannot be null.");
                }

                return await _unitOfWork.ProcessInTransactionAsync(async () =>
                {
                    var violation = (await _unitOfWork.GetRepository<Violation>()
                        .SingleOrDefaultAsync(
                            predicate: s => s.ViolationId == id
                        )).ValidateExists(id, "Can't update because this Violation isn't existed!");
                    

                    _mapper.Map(request, violation);


                    _unitOfWork.GetRepository<Violation>().UpdateAsync(violation);
                    await _unitOfWork.CommitAsync();

                    return _mapper.Map<ViolationListResponse>(violation);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating violation: {Message}", ex.Message);
                throw;
            }
        }
    }
}
