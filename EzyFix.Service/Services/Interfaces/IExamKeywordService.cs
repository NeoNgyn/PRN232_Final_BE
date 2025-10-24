using EzyFix.DAL.Data.Requests.ExamKeyword;
using EzyFix.DAL.Data.Responses.ExamKeyword;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzyFix.BLL.Services.Interfaces
{
    public interface IExamKeywordService
    {
        Task<IEnumerable<ExamKeywordResponse>> GetAllExamKeywordsAsync();
        Task<ExamKeywordResponse?> GetExamKeywordByIdAsync(Guid id);
        Task<IEnumerable<ExamKeywordResponse>> GetExamKeywordsByExamIdAsync(Guid examId);
        Task<IEnumerable<ExamKeywordResponse>> GetExamKeywordsByKeywordIdAsync(Guid keywordId);
        Task<ExamKeywordResponse> CreateExamKeywordAsync(CreateExamKeywordRequest createDto);
        Task<ExamKeywordResponse> UpdateExamKeywordAsync(Guid id, UpdateExamKeywordRequest updateDto);
        Task<bool> DeleteExamKeywordAsync(Guid id);
        Task<bool> DeleteExamKeywordsByExamIdAsync(Guid examId);
    }
}
