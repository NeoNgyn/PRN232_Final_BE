using EzyFix.DAL.Data.MetaDatas;
using EzyFix.DAL.Data.Requests.Subjects;
using EzyFix.DAL.Data.Responses.Subjects;
using EzyFix.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzyFix.BLL.Services.Interfaces
{
    public interface ISubjectService
    {
        Task<IEnumerable<SubjectResponse>> GetAllSubjectsAsync();
        Task<SubjectResponse?> GetSubjectByIdAsync(Guid id);
        Task<SubjectResponse> CreateSubjectAsync(CreateSubjectRequest createDto);
        Task<SubjectResponse> UpdateSubjectAsync(Guid id, UpdateSubjectRequest updateDto);
        Task<bool> DeleteSubjectAsync(Guid id);
    }
}
