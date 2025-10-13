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
        Task<IEnumerable<SemesterResponse>> GetAllSemestersAsync();
        Task<SemesterResponse?> GetSemesterByIdAsync(Guid id);
        Task<SemesterResponse> CreateSemesterAsync(CreateSemesterRequest createDto);
        Task<SemesterResponse> UpdateSemesterAsync(Guid id, UpdateSemesterRequest updateDto);
        Task<bool> DeleteSemesterAsync(Guid id);
    }
}
