using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzyFix.DAL.Data.Responses.ExamGradingCriteria
{
    public class ExamGradingCriterionResponse
    {
        public Guid CriteriaId { get; set; }
        public Guid ExamId { get; set; }
        public Guid ColumnId { get; set; }
        public decimal MaxMark { get; set; }
        public string Description { get; set; }
    }
}
