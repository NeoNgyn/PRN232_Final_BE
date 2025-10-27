using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzyFix.DAL.Data.Responses.Assignments
{
    public class AssignmentResponse
    {
        public Guid AssignmentId { get; set; }
        public string Name { get; set; }
        public string FilePath { get; set; }
        public DateTime? SubmissionDate { get; set; }
        public DateTime? Deadline { get; set; }
        public string Status { get; set; }

        // Student information
        public Guid StudentId { get; set; }
        public string StudentName { get; set; }
        public string StudentEmail { get; set; }

        // Exam information
        public Guid ExamId { get; set; }
        public string ExamTitle { get; set; }

        // Metadata
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
