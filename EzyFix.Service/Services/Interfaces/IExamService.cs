using EzyFix.DAL.Data.Requests.Exams;
using EzyFix.DAL.Data.Responses.Exams;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EzyFix.BLL.Services.Interfaces
{
    public interface IExamService
    {
        Task<IEnumerable<ExamResponse>> GetAllExamsAsync();
        Task<ExamResponse?> GetExamByIdAsync(Guid id);
        Task<ExamResponse> CreateExamAsync(CreateExamRequest createDto);
        Task<ExamResponse> UpdateExamAsync(Guid id, UpdateExamRequest updateDto);
        Task<bool> DeleteExamAsync(Guid id);
    }
}
