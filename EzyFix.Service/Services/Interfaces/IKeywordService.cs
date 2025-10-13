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
        Task<IEnumerable<KeywordResponseDto>> GetAllKeywordsAsync();
        Task<KeywordResponseDto?> GetKeywordByIdAsync(int id);
        Task<KeywordResponseDto> CreateKeywordAsync(CreateKeywordRequestDto createDto);
        Task<KeywordResponseDto> UpdateKeywordAsync(int id, UpdateKeywordRequestDto updateDto);
        Task<bool> DeleteKeywordAsync(int id);
    }
}
