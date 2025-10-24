using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzyFix.DAL.Data.Requests.GradingDetails
{
    public class UpdateGradingDetailRequest
    {
        public Guid? ScoreId { get; set; }

        public Guid? ColumnId { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "?i?m ph?i l?n h?n ho?c b?ng 0.")]
        public decimal? Mark { get; set; }
    }
}