using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzyFix.DAL.Data.Requests.ScoreColumns
{
    public class CreateScoreColumnRequest
    {
        [Required(ErrorMessage = "The name column cannot be left blank.")]
        [StringLength(100, ErrorMessage = "Score column names cannot exceed 100 characters.")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Description must not exceed 500 characters.")]
        public string Description { get; set; }
    }
}
