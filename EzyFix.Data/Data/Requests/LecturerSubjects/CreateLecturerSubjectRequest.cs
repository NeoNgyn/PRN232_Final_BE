using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzyFix.DAL.Data.Requests.LecturerSubjects
{
    public class CreateLecturerSubjectRequest
    {
        [Required(ErrorMessage = "LecturerId không ???c ?? tr?ng.")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "SubjectId không ???c ?? tr?ng.")]
        public Guid SubjectId { get; set; }

        [Required(ErrorMessage = "SemesterId không ???c ?? tr?ng.")]
        public Guid SemesterId { get; set; }

        public bool? IsPrincipal { get; set; }
    }
}