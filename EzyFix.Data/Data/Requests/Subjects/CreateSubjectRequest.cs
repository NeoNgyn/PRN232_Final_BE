using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzyFix.DAL.Data.Requests.Subjects
{
    public class CreateSubjectRequest
    {
        [Required(ErrorMessage = "Tên môn học không được để trống.")]
        [StringLength(100, ErrorMessage = "Tên môn học không được vượt quá 100 ký tự.")]
        public string Name { get; set; }
    }
}
