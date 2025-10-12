using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzyFix.DAL.Data.Requests.Semesters
{
    public class CreateSemesterRequestDto
    {
        [Required(ErrorMessage = "Mã học kỳ không được để trống.")]
        [StringLength(20, ErrorMessage = "Mã học kỳ không được vượt quá 20 ký tự.")]
        public string SemesterId { get; set; }
        [Required(ErrorMessage = "Tên học kỳ không được để trống.")]
        [StringLength(100, ErrorMessage = "Tên học kỳ không được vượt quá 100 ký tự.")]
        public string Name { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StartDate.HasValue && EndDate.HasValue && EndDate < StartDate)
            {
                yield return new ValidationResult(
                    "Ngày kết thúc không được nhỏ hơn ngày bắt đầu.",
                    new[] { nameof(EndDate), nameof(StartDate) });
            }
        }
    }
}
