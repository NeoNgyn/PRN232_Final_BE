using System;

namespace EzyFix.DAL.Data.Requests.Exams
{
    public class UpdateExamRequest
    {
        public string Title { get; set; }
        public Guid LecturerSubjectId { get; set; }
        public string FilePath { get; set; }         // optional: set by upload
        public string ExtractedPath { get; set; }    // optional
    }
}
