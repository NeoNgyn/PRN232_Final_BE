using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademicService.DAL.Models;

[Table("Criteria")]
public partial class Criteria
{
    [Key]
    public Guid CriteriaId { get; set; } = Guid.NewGuid();

    public Guid ExamId { get; set; }

    [Required]
    [StringLength(500)]
    public string CriteriaName { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal MaxScore { get; set; }

    public int SortOrder { get; set; }

    [ForeignKey("ExamId")]
    public virtual Exam Exam { get; set; } = null!;

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
}
