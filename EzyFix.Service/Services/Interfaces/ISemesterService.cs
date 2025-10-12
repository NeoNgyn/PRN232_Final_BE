using EzyFix.DAL.Data.MetaDatas;
using EzyFix.DAL.Data.Requests.Semesters;
using EzyFix.DAL.Data.Responses.Semesters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzyFix.BLL.Services.Interfaces
{
    public interface ISemesterService
    {
        Task<ApiResponse<IEnumerable<SemesterResponseDto>>> GetAllSemestersAsync();
        Task<ApiResponse<SemesterResponseDto?>> GetSemesterByIdAsync(string id);
        Task<ApiResponse<SemesterResponseDto>> CreateSemesterAsync(CreateSemesterRequestDto createDto);
        Task<ApiResponse<bool>> UpdateSemesterAsync(string id, UpdateSemesterRequestDto updateDto);
        Task<ApiResponse<bool>> DeleteSemesterAsync(string id);
    }
}
