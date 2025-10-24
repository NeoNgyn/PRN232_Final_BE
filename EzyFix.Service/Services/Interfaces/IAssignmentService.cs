using EzyFix.DAL.Data.Requests.Assignments;
using EzyFix.DAL.Data.Responses.Assignments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzyFix.BLL.Services.Interfaces
{
    public interface IAssignmentService
    {
        Task<IEnumerable<AssignmentResponse>> GetAllAssignmentsAsync();
        Task<AssignmentResponse?> GetAssignmentByIdAsync(Guid id);
        Task<AssignmentResponse> CreateAssignmentAsync(CreateAssignmentRequest createDto);
        Task<AssignmentResponse> UpdateAssignmentAsync(Guid id, UpdateAssignmentRequest updateDto);
        Task<bool> DeleteAssignmentAsync(Guid id);
        Task<IEnumerable<AssignmentResponse>> GetAssignmentsByStudentAsync(Guid studentId);
        Task<IEnumerable<AssignmentResponse>> GetAssignmentsByExamAsync(Guid examId);
    }
}
