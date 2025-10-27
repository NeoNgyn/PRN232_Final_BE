using System;

namespace EzyFix.DAL.Data.Responses.Exams
{
    public class ExamResponse
    {
        public Guid ExamId { get; set; }
        public string Title { get; set; }
        public string FilePath { get; set; }
        public string ExtractedPath { get; set; }
        public DateTime? UploadedAt { get; set; }
        public Guid LecturerSubjectId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
