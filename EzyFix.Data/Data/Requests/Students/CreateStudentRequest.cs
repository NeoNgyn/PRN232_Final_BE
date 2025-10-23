using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzyFix.DAL.Data.Requests.Students
{
    public class CreateStudentRequest
    {
        [Required(ErrorMessage = "Tên sinh viên không ???c ?? tr?ng.")]
        [StringLength(50, ErrorMessage = "Tên sinh viên không ???c v??t quá 50 ký t?.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email không ???c ?? tr?ng.")]
        [EmailAddress(ErrorMessage = "Email không ?úng ??nh d?ng.")]
        [StringLength(50, ErrorMessage = "Email không ???c v??t quá 50 ký t?.")]
        public string Email { get; set; }
    }
}