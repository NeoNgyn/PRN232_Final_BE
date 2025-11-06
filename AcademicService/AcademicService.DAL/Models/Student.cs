using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademicService.DAL.Models;

[Table("Students")]
public partial class Student
{
    [Key]
    public string StudentId { get; set; } 

    [Required]
    [StringLength(255)]
    public string FullName { get; set; } = string.Empty;

    public string? Status { get; set; }

    public virtual ICollection<Submission> Submissions { get; set; } = new List<Submission>();
}
