using EzyFix.DAL.Data.Requests.ScoreColumns;
using EzyFix.DAL.Data.Responses.ScoreColumns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzyFix.BLL.Services.Interfaces
{
    public interface IScoreColumnService
    {
        Task<IEnumerable<ScoreColumnResponse>> GetAllScoreColumnsAsync();
        Task<ScoreColumnResponse?> GetScoreColumnByIdAsync(Guid id);
        Task<ScoreColumnResponse> CreateScoreColumnAsync(CreateScoreColumnRequest createDto);
        Task<ScoreColumnResponse> UpdateScoreColumnAsync(Guid id, UpdateScoreColumnRequest updateDto);
        Task<bool> DeleteScoreColumnAsync(Guid id);
    }
}
