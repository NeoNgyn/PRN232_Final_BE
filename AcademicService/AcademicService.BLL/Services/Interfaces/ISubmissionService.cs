using AcademicService.DAL.Data.Requests;
using AcademicService.DAL.Data.Requests.Submission;
using AcademicService.DAL.Data.Responses;
using AcademicService.DAL.Data.Responses.Submission;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicService.BLL.Services.Interfaces
{
    public interface ISubmissionService
    {
        Task<IEnumerable<SubmissionListResponse>> GetAllSubmissionsAsync();
        Task<IEnumerable<SubmissionDetailResponse>> GetSubmissionsByExamIdAsync(Guid examId);
        Task<IEnumerable<SubmissionDetailResponse>> GetSubmissionsByExamIdAndExamninerIdAsync(Guid examId, Guid examinerId);
        Task<IEnumerable<SubmissionListResponse>> GetQuerySubmissionsAsync();
        Task<SubmissionDetailResponse> GetSubmissionByIdAsync(Guid id);
        Task<SubmissionListResponse> CreateSubmissionAsync(CreateSubmissionRequest request, IFormFile fileSubmit);
        Task<SubmissionListResponse> UpdateSubmissionAsync(Guid id, UpdateSubmissionRequest request);
        Task<bool> DeleteSubmissionAsync(Guid id);
    }
}
