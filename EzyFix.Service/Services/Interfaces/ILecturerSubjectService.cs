using EzyFix.DAL.Data.Requests.LecturerSubjects;
using EzyFix.DAL.Data.Responses.LecturerSubjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzyFix.BLL.Services.Interfaces
{
    public interface ILecturerSubjectService
    {
        Task<IEnumerable<LecturerSubjectResponse>> GetAllLecturerSubjectsAsync();
        Task<LecturerSubjectResponse?> GetLecturerSubjectByIdAsync(Guid id);
        Task<LecturerSubjectResponse> CreateLecturerSubjectAsync(CreateLecturerSubjectRequest createDto);
        Task<LecturerSubjectResponse> UpdateLecturerSubjectAsync(Guid id, UpdateLecturerSubjectRequest updateDto);
        Task<bool> DeleteLecturerSubjectAsync(Guid id);
        Task<IEnumerable<LecturerSubjectResponse>> GetLecturerSubjectsByLecturerIdAsync(Guid lecturerId);
        Task<IEnumerable<LecturerSubjectResponse>> GetLecturerSubjectsBySubjectIdAsync(Guid subjectId);
        Task<IEnumerable<LecturerSubjectResponse>> GetLecturerSubjectsBySemesterIdAsync(Guid semesterId);
    }
}