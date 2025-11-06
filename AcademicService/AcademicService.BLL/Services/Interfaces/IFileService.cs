using AcademicService.DAL.Data.Requests;

namespace AcademicService.BLL.Services.Interfaces;

public interface IFileService
{
    string GetFileUrl(string filePath);

    Task<IEnumerable<CreateStudentRequest>> ReadStudentsFromJsonAsync(string filePath);
}
