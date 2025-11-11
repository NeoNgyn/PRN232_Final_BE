using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicService.DAL.Data.Requests.Criteria
{
    public class CreateCriteriaRequest
    {
        public Guid ExamId { get; set; }

        [Required]
        [StringLength(500)]
        public string CriteriaName { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal MaxScore { get; set; }

        [Required]
        public int SortOrder { get; set; }
    }
}
