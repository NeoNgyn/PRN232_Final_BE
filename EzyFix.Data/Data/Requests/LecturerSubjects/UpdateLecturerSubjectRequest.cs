using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzyFix.DAL.Data.Requests.LecturerSubjects
{
    public class UpdateLecturerSubjectRequest
    {
        public Guid? LecturerId { get; set; }

        public Guid? SubjectId { get; set; }

        public Guid? SemesterId { get; set; }

        public bool? IsPrincipal { get; set; }
    }
}