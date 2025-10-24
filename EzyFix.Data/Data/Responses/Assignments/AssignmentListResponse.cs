using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzyFix.DAL.Data.Responses.Assignments
{
    public class AssignmentListResponse
    {
        public Guid AssignmentId { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public DateTime? SubmissionDate { get; set; }
        public DateTime? Deadline { get; set; }

        // Basic student information
        public Guid StudentId { get; set; }
        public string StudentName { get; set; }

        // Basic exam information
        public Guid ExamId { get; set; }
        public string ExamTitle { get; set; }
    }
}
