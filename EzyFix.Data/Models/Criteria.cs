using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzyFix.DAL.Models
{
    [Table("Criteria")]
    public partial class Criteria
    {
        [Key]
        public Guid CriteriaId { get; set; } = Guid.NewGuid(); // Đổi sang Guid

        public Guid ExamId { get; set; } // Đổi sang Guid

        [Required]
        [StringLength(500)]
        public string CriteriaName { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal MaxScore { get; set; }

        public int SortOrder { get; set; } // Giữ nguyên int

        [ForeignKey("ExamId")]
        public virtual Exam Exam { get; set; }

        public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
    }
}
