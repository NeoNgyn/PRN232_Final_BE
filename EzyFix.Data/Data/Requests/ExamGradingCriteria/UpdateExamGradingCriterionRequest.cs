using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzyFix.DAL.Data.Requests.ExamGradingCriteria
{
    public class UpdateExamGradingCriterionRequest
    {
        public Guid? ExamId { get; set; }

        public Guid? ColumnId { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "MaxMark ph?i l?n h?n ho?c b?ng 0.")]
        public decimal? MaxMark { get; set; }

        [StringLength(500, ErrorMessage = "Description không ???c v??t quá 500 ký t?.")]
        public string Description { get; set; }
    }
}