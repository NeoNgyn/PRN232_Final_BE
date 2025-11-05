using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademicService.DAL.Models;

[Table("Subjects")]
public partial class Subject
{
    [Key]
    public Guid SubjectId { get; set; } = Guid.NewGuid();

    [Required]
    [StringLength(20)]
    public string SubjectCode { get; set; } = string.Empty;

    [Required]
    [StringLength(255)]
    public string SubjectName { get; set; } = string.Empty;

    public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();
}
