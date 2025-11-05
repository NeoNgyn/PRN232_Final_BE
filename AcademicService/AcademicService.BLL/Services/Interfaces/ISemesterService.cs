using AcademicService.DAL.Data.Requests;
using AcademicService.DAL.Data.Responses;

namespace AcademicService.BLL.Services.Interfaces;

public interface ISemesterService
{
    Task<IEnumerable<SemesterResponse>> GetAllSemestersAsync();
    Task<SemesterResponse?> GetSemesterByIdAsync(Guid id);
    Task<SemesterResponse> CreateSemesterAsync(CreateSemesterRequest request);
    Task<SemesterResponse> UpdateSemesterAsync(Guid id, UpdateSemesterRequest request);
    Task DeleteSemesterAsync(Guid id);
}
