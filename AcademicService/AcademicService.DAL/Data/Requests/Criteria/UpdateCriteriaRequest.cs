using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicService.DAL.Data.Requests.Criteria
{
    public class UpdateCriteriaRequest
    {
        
        [StringLength(500)]
        public string? CriteriaName { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? MaxScore { get; set; }

        public int? SortOrder { get; set; }
    }
}
