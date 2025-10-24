using System;

namespace EzyFix.DAL.Data.Requests.Exams
{
    public class CreateExamRequest
    {
        public string Title { get; set; }
        public Guid LecturerSubjectId { get; set; }
    }
}
