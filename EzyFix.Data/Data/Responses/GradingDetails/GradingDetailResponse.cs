using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzyFix.DAL.Data.Responses.GradingDetails
{
    public class GradingDetailResponse
    {
        public Guid DetailId { get; set; }
        public Guid ScoreId { get; set; }
        public Guid ColumnId { get; set; }
        public decimal? Mark { get; set; }
        
        // Optional: Include related entity names for better UX
        public string ColumnName { get; set; }
        public string ScoreResultInfo { get; set; }
    }
}