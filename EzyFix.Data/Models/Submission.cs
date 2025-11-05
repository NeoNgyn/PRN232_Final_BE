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
    [Table("Submissions")]
    public partial class Submission
    {
        [Key]
        public Guid SubmissionId { get; set; } = Guid.NewGuid(); // Đổi sang Guid

        public Guid ExamId { get; set; } // Đổi sang Guid
        public Guid StudentId { get; set; } // Đổi sang Guid
        public Guid UploadedBy_UserID { get; set; } // Giữ nguyên Guid

        [Required]
        [StringLength(500)]
        public string OriginalFileName { get; set; }

        [Required]
        [StringLength(1000)]
        public string FilePath { get; set; }

        [Required]
        [StringLength(20)]
        public string GradingStatus { get; set; } = "Pending";

        [Column(TypeName = "timestamp with time zone")]
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("ExamId")]
        public virtual Exam Exam { get; set; }

        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; }

        [ForeignKey("UploadedBy_UserID")]
        public virtual User UploadedBy { get; set; }

        public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
        public virtual ICollection<Violation> Violations { get; set; } = new List<Violation>();
    }
}
