using AcademicService.DAL.Data.Requests;
using AcademicService.DAL.Data.Requests.Criteria;

namespace AcademicService.BLL.Services.Interfaces;

public interface IFileService
{
    string GetFileUrl(string filePath);

    Task<IEnumerable<CreateStudentRequest>> ReadStudentsFromJsonAsync(string filePath);

    Task<IEnumerable<CreateCriteriaRequest>> ReadCriteriasFromExcelAsync(string filePath, Guid examId);
}
