using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademicService.DAL.Models;

[Table("Exams")]
public partial class Exam
{
    [Key]
    public Guid ExamId { get; set; } = Guid.NewGuid();

    public Guid SubjectId { get; set; }
    public Guid SemesterId { get; set; }

    [Required]
    [StringLength(255)]
    public string ExamName { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string ExamType { get; set; } = string.Empty;

    [StringLength(50)]
    public string? ExamPassword { get; set; }

    [Column(TypeName = "timestamp with time zone")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey("SubjectId")]
    public virtual Subject Subject { get; set; } = null!;

    [ForeignKey("SemesterId")]
    public virtual Semester Semester { get; set; } = null!;

    public virtual ICollection<TeacherAssignment> TeacherAssignments { get; set; } = new List<TeacherAssignment>();
    public virtual ICollection<Criteria> Criteria { get; set; } = new List<Criteria>();
    public virtual ICollection<Submission> Submissions { get; set; } = new List<Submission>();
}
