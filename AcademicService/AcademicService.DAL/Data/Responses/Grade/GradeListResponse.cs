using AcademicService.DAL.Data.Responses.Criteria;
using AcademicService.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicService.DAL.Data.Responses.Grade
{
    public class GradeListResponse
    {
        public Guid GradeId { get; set; } 

        public Guid SubmissionId { get; set; }
        public Guid CriteriaId { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Score { get; set; }

        public string? Note { get; set; }

        public CriteriaListResponse Criteria { get; set; } = null!;
    }
}
