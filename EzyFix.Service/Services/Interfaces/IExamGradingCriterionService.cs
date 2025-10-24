using EzyFix.DAL.Data.Requests.ExamGradingCriteria;
using EzyFix.DAL.Data.Responses.ExamGradingCriteria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzyFix.BLL.Services.Interfaces
{
    public interface IExamGradingCriterionService
    {
        Task<IEnumerable<ExamGradingCriterionResponse>> GetAllExamGradingCriteriaAsync();
        Task<ExamGradingCriterionResponse?> GetExamGradingCriterionByIdAsync(Guid id);
        Task<ExamGradingCriterionResponse> CreateExamGradingCriterionAsync(CreateExamGradingCriterionRequest createDto);
        Task<ExamGradingCriterionResponse> UpdateExamGradingCriterionAsync(Guid id, UpdateExamGradingCriterionRequest updateDto);
        Task<bool> DeleteExamGradingCriterionAsync(Guid id);
    }
}