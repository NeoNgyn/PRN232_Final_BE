using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzyFix.DAL.Data.Requests.Assignments
{
    public class UpdateAssignmentRequest
    {
        [Required(ErrorMessage = "Tên bài tập không được để trống.")]
        [StringLength(100, ErrorMessage = "Tên bài tập không được vượt quá 100 ký tự.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Đường dẫn file không được để trống.")]
        [StringLength(255, ErrorMessage = "Đường dẫn file không được vượt quá 255 ký tự.")]
        public string FilePath { get; set; }

        public DateTime? SubmissionDate { get; set; }

        public DateTime? Deadline { get; set; }

        [StringLength(20, ErrorMessage = "Trạng thái không được vượt quá 20 ký tự.")]
        public string Status { get; set; }

        [Required(ErrorMessage = "ID sinh viên không được để trống.")]
        public Guid StudentId { get; set; }

        [Required(ErrorMessage = "ID bài kiểm tra không được để trống.")]
        public Guid ExamId { get; set; }
    }
}
