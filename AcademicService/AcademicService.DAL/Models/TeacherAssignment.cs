using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademicService.DAL.Models;

[Table("TeacherAssignments")]
public partial class TeacherAssignment
{
    [Key]
    public Guid AssignmentId { get; set; } = Guid.NewGuid();

    public Guid UserID_Teacher { get; set; }
    public Guid ExamId { get; set; }

    // Note: Teacher/User is in IdentityService
    // We only store the Guid reference

    [ForeignKey("ExamId")]
    public virtual Exam Exam { get; set; } = null!;
}
