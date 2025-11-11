using AcademicService.DAL.Data.Requests.Criteria;
using AcademicService.DAL.Data.Requests.Grade;
using AcademicService.DAL.Data.Responses.Criteria;
using AcademicService.DAL.Data.Responses.Grade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicService.BLL.Services.Interfaces
{
    public interface IGradeService
    {
        Task<IEnumerable<GradeListResponse>> GetAllGradesAsync();
        Task<GradeListResponse> GetGradeByIdAsync(Guid id);
        Task<GradeListResponse> CreateGradeAsync(CreateGradeRequest request);
        Task<GradeListResponse> UpdateGradeAsync(Guid id, UpdateGradeRequest request);
        Task<bool> DeleteGradeAsync(Guid id);
    }
}
