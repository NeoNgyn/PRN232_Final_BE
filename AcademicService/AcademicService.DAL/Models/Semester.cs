using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademicService.DAL.Models;

[Table("Semesters")]
public partial class Semester
{
    [Key]
    public Guid SemesterId { get; set; } = Guid.NewGuid();

    [Required]
    [StringLength(20)]
    public string SemesterCode { get; set; } = string.Empty;

    [StringLength(100)]
    public string? SemesterName { get; set; }

    public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();
}
