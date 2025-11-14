using AcademicService.DAL.Data.Requests.Criteria;
using AcademicService.DAL.Data.Requests.Violation;
using AcademicService.DAL.Data.Responses.Criteria;
using AcademicService.DAL.Data.Responses.Violation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicService.BLL.Services.Interfaces
{
    public interface IViolationService 
    {
        Task<IEnumerable<ViolationListResponse>> GetAllViolationsAsync();
        Task<IEnumerable<ViolationListResponse>> GetQueryViolationsAsync();
        Task<ViolationListResponse> GetViolationByIdAsync(Guid id);
        Task<ViolationListResponse> CreateViolationAsync(CreateViolationRequest request);
        Task<ViolationListResponse> UpdateViolationAsync(Guid id, UpdateViolationRequest request);
        Task<bool> DeleteViolationAsync(Guid id);
    }
}
