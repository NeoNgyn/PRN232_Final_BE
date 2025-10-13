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
        Task<IEnumerable<KeywordResponse>> GetAllKeywordsAsync();
        Task<KeywordResponse?> GetKeywordByIdAsync(Guid id);
        Task<KeywordResponse> CreateKeywordAsync(CreateKeywordRequest createDto);
        Task<KeywordResponse> UpdateKeywordAsync(Guid id, UpdateKeywordRequest updateDto);
        Task<bool> DeleteKeywordAsync(Guid id);
    }
}
