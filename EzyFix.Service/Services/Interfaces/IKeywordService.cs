using EzyFix.DAL.Data.MetaDatas;
using EzyFix.DAL.Data.Requests.Keywords;
using EzyFix.DAL.Data.Responses.Keywords;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzyFix.BLL.Services.Interfaces
{
    public interface IKeywordService
    {
        Task<ApiResponse<IEnumerable<KeywordResponseDto>>> GetAllKeywordsAsync();
        Task<ApiResponse<KeywordResponseDto?>> GetKeywordByIdAsync(int keywordId);
        Task<ApiResponse<KeywordResponseDto>> CreateKeywordAsync(CreateKeywordRequestDto createDto);
        Task<ApiResponse<bool>> UpdateKeywordAsync(int keywordId, UpdateKeywordRequestDto updateDto);
        Task<ApiResponse<bool>> DeleteKeywordAsync(int keywordId);
    }
}
