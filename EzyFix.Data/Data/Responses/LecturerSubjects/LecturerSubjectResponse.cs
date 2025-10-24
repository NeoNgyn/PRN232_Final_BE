using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzyFix.DAL.Data.Responses.LecturerSubjects
{
    public class LecturerSubjectResponse
    {
        public Guid LecturerSubjectId { get; set; }
        public Guid LecturerId { get; set; }
        public Guid SubjectId { get; set; }
        public Guid SemesterId { get; set; }
        public bool? IsPrincipal { get; set; }
        public DateTime? AssignedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        
        // Optional: Include related entity names for better UX
        public string LecturerName { get; set; }
        public string SubjectName { get; set; }
        public string SemesterName { get; set; }
        public int ExamCount { get; set; } = 0; // Count of exams for this assignment
    }
}