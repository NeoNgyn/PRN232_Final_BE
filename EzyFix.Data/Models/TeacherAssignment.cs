using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzyFix.DAL.Models
{
    [Table("TeacherAssignments")]
    public partial class TeacherAssignment
    {
        [Key]
        public Guid AssignmentId { get; set; } = Guid.NewGuid(); // Đổi sang Guid

        public Guid UserID_Teacher { get; set; } // Giữ nguyên Guid
        public Guid ExamId { get; set; } // Đổi sang Guid

        [ForeignKey("UserID_Teacher")]
        public virtual User Teacher { get; set; }

        [ForeignKey("ExamId")]
        public virtual Exam Exam { get; set; }
    }
}
