using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicService.DAL.Data.Responses.Submission
{
    public class SubmissionListResponse
    {
        public Guid SubmissionId { get; set; } 
        public Guid ExamId { get; set; }
        public string StudentId { get; set; } = string.Empty;
        public Guid ExaminerId { get; set; }
        public Guid? SecondExaminerId { get; set; }
        public string OriginalFileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string GradingStatus { get; set; } = "Pending";
        [Precision(18, 2)]
        public decimal? TotalScore { get; set; }
        [Column(TypeName = "timestamp with time zone")]
        public DateTime UploadedAt { get; set; } 
        public bool IsApproved { get; set; } = false;
    }
}
