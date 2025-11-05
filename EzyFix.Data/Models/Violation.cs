using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzyFix.DAL.Models
{
    [Table("Violations")]
    public partial class Violation
    {
        [Key]
        public Guid ViolationId { get; set; } = Guid.NewGuid(); // Đổi sang Guid

        public Guid SubmissionId { get; set; } // Đổi sang Guid

        [Required]
        [StringLength(50)]
        public string Type { get; set; }

        public string Description { get; set; }

        [StringLength(20)]
        public string Severity { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal? Penalty { get; set; }

        [Column(TypeName = "timestamp with time zone")]
        public DateTime DetectedAt { get; set; } = DateTime.UtcNow;

        public Guid? DetectedBy_UserID { get; set; } // Giữ nguyên Guid?

        public bool Resolved { get; set; } = false;

        [ForeignKey("SubmissionId")]
        public virtual Submission Submission { get; set; }

        [ForeignKey("DetectedBy_UserID")]
        public virtual User DetectedBy { get; set; }
    }
}
