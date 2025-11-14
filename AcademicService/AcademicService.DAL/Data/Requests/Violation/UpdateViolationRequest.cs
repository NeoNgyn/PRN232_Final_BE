using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicService.DAL.Data.Requests.Violation
{
    public class UpdateViolationRequest
    {
        [Required]
        [StringLength(50)]
        public string Type { get; set; } = string.Empty;

        public string? Description { get; set; }

        [StringLength(20)]
        public string? Severity { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Penalty { get; set; }

        public Guid? DetectedBy_UserID { get; set; }

        public bool Resolved { get; set; } = false;
    }
}
