using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicService.DAL.Data.Requests.Submission
{
    public class UpdateSubmissionRequest
    {
        // ExamId KHÔNG được phép thay đổi sau khi tạo submission
        // public Guid? ExamId { get; set; }
        public Guid? SecondExaminerId { get; set; }
        // StudentId KHÔNG nên thay đổi sau khi tạo submission
        // public string? StudentId { get; set; }
        public string? GradingStatus { get; set; }
        public decimal? TotalScore { get; set; }
        public bool? IsApproved { get; set; }
    }
}
