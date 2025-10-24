using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzyFix.DAL.Data.Responses.ScoreColumns
{
    public class ScoreColumnResponse
    {
        public Guid ColumnId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
