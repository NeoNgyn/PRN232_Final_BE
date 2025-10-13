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
        Task<IEnumerable<SubjectResponseDto>> GetAllSubjectsAsync();
        Task<SubjectResponseDto?> GetSubjectByIdAsync(string id);
        Task<SubjectResponseDto> CreateSubjectAsync(CreateSubjectRequestDto createDto);
        Task<SubjectResponseDto> UpdateSubjectAsync(string id, UpdateSubjectRequestDto updateDto);
        Task<bool> DeleteSubjectAsync(string id);
    }
}
