using AcademicService.DAL.Data.Requests;
using AcademicService.DAL.Data.Responses;

namespace AcademicService.BLL.Services.Interfaces;

public interface IExamService
{
    Task<IEnumerable<ExamResponse>> GetAllExamsAsync();
    Task<ExamResponse?> GetExamByIdAsync(Guid id);
    Task<ExamResponse> CreateExamAsync(CreateExamRequest request);
    Task<ExamResponse> UpdateExamAsync(Guid id, UpdateExamRequest request);
    Task DeleteExamAsync(Guid id);
}
