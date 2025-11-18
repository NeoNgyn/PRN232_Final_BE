using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicService.DAL.Data.Requests.Submission
{
    public class CreateSubmissionRequest
    {
        [Required]
        public Guid ExamId { get; set; }
        [Required]
        public Guid ExaminerId { get; set; }
        public string? StudentId { get; set; }
    }
}
