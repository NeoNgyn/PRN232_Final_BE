using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzyFix.DAL.Models
{
    [Table("Grades")]
    public partial class Grade
    {
        [Key]
        public Guid GradeId { get; set; } = Guid.NewGuid(); // Đổi sang Guid

        public Guid SubmissionId { get; set; } // Đổi sang Guid
        public Guid CriteriaId { get; set; } // Đổi sang Guid

        [Column(TypeName = "decimal(5, 2)")]
        public decimal Score { get; set; }

        public string Note { get; set; }

        [ForeignKey("SubmissionId")]
        public virtual Submission Submission { get; set; }

        [ForeignKey("CriteriaId")]
        public virtual Criteria Criteria { get; set; }
    }
}
