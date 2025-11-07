using AcademicService.DAL.Data.Requests.Criteria;
using AcademicService.DAL.Data.Requests.Submission;
using AcademicService.DAL.Data.Responses.Criteria;
using AcademicService.DAL.Data.Responses.Submission;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicService.BLL.Services.Interfaces
{
    public interface ICritieriaService
    {
        Task<IEnumerable<CriteriaListResponse>> GetAllCriteriasAsync(CriteriaQueryParameter queryParameter);
        Task<IEnumerable<CriteriaListResponse>> GetQueryCriteriasAsync();
        Task<CriteriaListResponse> GetCriteriaByIdAsync(Guid id);
        Task<CriteriaListResponse> CreateCriteriaAsync(CreateCriteriaRequest request);
        Task<CriteriaListResponse> UpdateCriteriaAsync(Guid id, UpdateCriteriaRequest request);
        Task<bool> DeleteCriteriaAsync(Guid id);
    }
}
