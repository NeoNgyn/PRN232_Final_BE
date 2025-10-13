using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzyFix.DAL.Data.Requests.Semesters
{
    public class CreateSemesterRequest
    {
        [Required(ErrorMessage = "Tên học kỳ không được để trống.")]
        [StringLength(100, ErrorMessage = "Tên học kỳ không được vượt quá 100 ký tự.")]
        public string Name { get; set; }

        public DateOnly? StartDate { get; set; }

        public DateOnly? EndDate { get; set; }
    }
}
