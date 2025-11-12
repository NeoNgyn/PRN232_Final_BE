using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademicService.DAL.Models;

[Table("Violations")]
public partial class Violation
{
    [Key]
    public Guid ViolationId { get; set; } = Guid.NewGuid();

    public Guid SubmissionId { get; set; }

    [Required]
    [StringLength(50)]
    public string Type { get; set; } = string.Empty;

    public string? Description { get; set; }

    [StringLength(20)]
    public string? Severity { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Penalty { get; set; }

    [Column(TypeName = "timestamp with time zone")]
    public DateTime DetectedAt { get; set; } = DateTime.UtcNow;

    public Guid? DetectedBy_UserID { get; set; }

    public bool Resolved { get; set; } = false;

    [ForeignKey("SubmissionId")]
    public virtual Submission Submission { get; set; } = null!;

    // Note: DetectedBy/User is in IdentityService
}
