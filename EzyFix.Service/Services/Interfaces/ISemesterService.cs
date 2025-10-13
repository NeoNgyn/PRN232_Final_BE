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
        Task<IEnumerable<SemesterResponseDto>> GetAllSemestersAsync();
        Task<SemesterResponseDto?> GetSemesterByIdAsync(string id);
        Task<SemesterResponseDto> CreateSemesterAsync(CreateSemesterRequestDto createDto);
        Task<SemesterResponseDto> UpdateSemesterAsync(string id, UpdateSemesterRequestDto updateDto);
        Task<bool> DeleteSemesterAsync(string id);
    }
}
