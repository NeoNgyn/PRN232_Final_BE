using EzyFix.DAL.Data.MetaDatas;
using EzyFix.DAL.Data.Requests.Subjects;
using EzyFix.DAL.Data.Responses.Subjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzyFix.BLL.Services.Interfaces
{
    public interface ISubjectService
    {
        Task<ApiResponse<IEnumerable<SubjectResponseDto>>> GetAllSubjectsAsync();
        Task<ApiResponse<SubjectResponseDto?>> GetSubjectByIdAsync(string subjectId);
        Task<ApiResponse<SubjectResponseDto>> CreateSubjectAsync(CreateSubjectRequestDto createSubjectDto);
        Task<ApiResponse<bool>> UpdateSubjectAsync(string subjectId, UpdateSubjectRequestDto updateSubjectDto);
        Task<ApiResponse<bool>> DeleteSubjectAsync(string subjectId);
    }
}
