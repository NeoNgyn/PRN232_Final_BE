using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademicService.DAL.Models;

[Table("Submissions")]
public partial class Submission
{
    [Key]
    public Guid SubmissionId { get; set; } = Guid.NewGuid();

    public Guid ExamId { get; set; }
    public Guid StudentId { get; set; }
    public Guid ExaminerId { get; set; }
    public Guid? SecondExaminerId { get; set; }

    [Required]
    [StringLength(500)]
    public string OriginalFileName { get; set; } = string.Empty;

    [Required]
    [StringLength(1000)]
    public string FilePath { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string GradingStatus { get; set; } = "Pending";

    [Column(TypeName = "timestamp with time zone")]
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

    public bool IsApproved { get; set; } = false;

    [ForeignKey("ExamId")]
    public virtual Exam Exam { get; set; } = null!;

    [ForeignKey("StudentId")]
    public virtual Student Student { get; set; } = null!;

    // Note: User is in IdentityService, so we only store the Guid
    // We won't have navigation property here to avoid cross-service dependencies

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
    public virtual ICollection<Violation> Violations { get; set; } = new List<Violation>();
}
