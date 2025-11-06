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
        public Guid? ExamId { get; set; }
        public Guid? SecondExaminerId { get; set; }
        public string? StudentId { get; set; } = string.Empty;
        public string? GradingStatus { get; set; }
        public decimal? TotalScore { get; set; }
    }
}
