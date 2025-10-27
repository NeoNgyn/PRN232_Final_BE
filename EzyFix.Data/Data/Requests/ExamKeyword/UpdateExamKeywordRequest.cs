using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzyFix.DAL.Data.Requests.ExamKeyword
{
    public class UpdateExamKeywordRequest
    {
        [Required(ErrorMessage = "Exam ID is required")]
        public Guid ExamId { get; set; }

        [Required(ErrorMessage = "Keyword ID is required")]
        public Guid KeywordId { get; set; }
    }
}
