using AcademicService.DAL.Data.Requests;
using AcademicService.DAL.Data.Responses;

namespace AcademicService.BLL.Services.Interfaces;

public interface IStudentService
{
    Task<IEnumerable<StudentResponse>> GetAllStudentsAsync();
    Task<StudentResponse?> GetStudentByIdAsync(Guid id);
    Task<StudentResponse> CreateStudentAsync(CreateStudentRequest request);
    Task<StudentResponse> UpdateStudentAsync(Guid id, UpdateStudentRequest request);
    Task DeleteStudentAsync(Guid id);

    Task<IEnumerable<StudentResponse>> ImportStudentsFromFileAsync(string filePath, IFileService fileService);
}
