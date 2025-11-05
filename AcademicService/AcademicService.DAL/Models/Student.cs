using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademicService.DAL.Models;

[Table("Students")]
public partial class Student
{
    [Key]
    public Guid StudentId { get; set; } = Guid.NewGuid();

    [Required]
    [StringLength(20)]
    public string StudentMSSV { get; set; } = string.Empty;

    [Required]
    [StringLength(255)]
    public string FullName { get; set; } = string.Empty;

    public virtual ICollection<Submission> Submissions { get; set; } = new List<Submission>();
}
