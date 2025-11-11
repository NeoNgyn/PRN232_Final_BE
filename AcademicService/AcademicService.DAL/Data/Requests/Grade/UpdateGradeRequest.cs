using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicService.DAL.Data.Requests.Grade
{
    public class UpdateGradeRequest
    {
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Score { get; set; }

        public string? Note { get; set; }
    }
}
