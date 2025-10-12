using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzyFix.DAL.Data.Requests.Keywords
{
    public class CreateKeywordRequestDto
    {
        [Required(ErrorMessage = "Từ khóa không được để trống.")]
        [StringLength(100, ErrorMessage = "Từ khóa không được vượt quá 100 ký tự.")]
        public string Word { get; set; }
    }
}
