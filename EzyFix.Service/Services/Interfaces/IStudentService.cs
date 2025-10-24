using EzyFix.DAL.Data.Requests.Students;
using EzyFix.DAL.Data.Responses.Students;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzyFix.BLL.Services.Interfaces
{
    public interface IStudentService
    {
        Task<IEnumerable<StudentResponse>> GetAllStudentsAsync();
        Task<StudentResponse?> GetStudentByIdAsync(Guid id);
        Task<StudentResponse> CreateStudentAsync(CreateStudentRequest createDto);
        Task<StudentResponse> UpdateStudentAsync(Guid id, UpdateStudentRequest updateDto);
        Task<bool> DeleteStudentAsync(Guid id);
    }
}