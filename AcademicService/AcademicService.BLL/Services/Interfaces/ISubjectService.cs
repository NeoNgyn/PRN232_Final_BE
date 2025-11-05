using AcademicService.DAL.Data.Requests;
using AcademicService.DAL.Data.Responses;

namespace AcademicService.BLL.Services.Interfaces;

public interface ISubjectService
{
    Task<IEnumerable<SubjectResponse>> GetAllSubjectsAsync();
    Task<SubjectResponse?> GetSubjectByIdAsync(Guid id);
    Task<SubjectResponse> CreateSubjectAsync(CreateSubjectRequest request);
    Task<SubjectResponse> UpdateSubjectAsync(Guid id, UpdateSubjectRequest request);
    Task DeleteSubjectAsync(Guid id);
}
