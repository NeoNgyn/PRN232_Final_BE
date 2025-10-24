using EzyFix.DAL.Data.Requests.GradingDetails;
using EzyFix.DAL.Data.Responses.GradingDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzyFix.BLL.Services.Interfaces
{
    public interface IGradingDetailService
    {
        Task<IEnumerable<GradingDetailResponse>> GetAllGradingDetailsAsync();
        Task<GradingDetailResponse?> GetGradingDetailByIdAsync(Guid id);
        Task<GradingDetailResponse> CreateGradingDetailAsync(CreateGradingDetailRequest createDto);
        Task<GradingDetailResponse> UpdateGradingDetailAsync(Guid id, UpdateGradingDetailRequest updateDto);
        Task<bool> DeleteGradingDetailAsync(Guid id);
        Task<IEnumerable<GradingDetailResponse>> GetGradingDetailsByScoreIdAsync(Guid scoreId);
        Task<IEnumerable<GradingDetailResponse>> GetGradingDetailsByColumnIdAsync(Guid columnId);
    }
}