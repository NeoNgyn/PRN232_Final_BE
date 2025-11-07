using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademicService.DAL.Models;

[Table("Grades")]
public partial class Grade
{
    [Key]
    public Guid GradeId { get; set; } 

    public Guid SubmissionId { get; set; }
    public Guid CriteriaId { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Score { get; set; }

    public string? Note { get; set; }

    [ForeignKey("SubmissionId")]
    public virtual Submission Submission { get; set; } = null!;

    [ForeignKey("CriteriaId")]
    public virtual Criteria Criteria { get; set; } = null!;
}
