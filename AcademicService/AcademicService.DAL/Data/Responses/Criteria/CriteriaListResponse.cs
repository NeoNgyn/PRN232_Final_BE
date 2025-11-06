using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicService.DAL.Data.Responses.Criteria
{
    public class CriteriaListResponse
    {
        public Guid CriteriaId { get; set; } 

        public Guid ExamId { get; set; }

        
        public string CriteriaName { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18, 2)")]
        public decimal MaxScore { get; set; }

        public int SortOrder { get; set; }
    }
}
