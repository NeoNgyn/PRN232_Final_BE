using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicService.DAL.Data.Responses.Violation
{
    public class ViolationListResponse
    {
        public Guid ViolationId { get; set; }

        public Guid SubmissionId { get; set; }

        
        public string Type { get; set; } 

        public string? Description { get; set; }

        
        public string? Severity { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Penalty { get; set; }

        
        public DateTime DetectedAt { get; set; } 

        public Guid? DetectedBy_UserID { get; set; }

        public bool Resolved { get; set; } = false;
    }
}
