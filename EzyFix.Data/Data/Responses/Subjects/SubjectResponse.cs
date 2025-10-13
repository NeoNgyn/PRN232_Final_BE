using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzyFix.DAL.Data.Responses.Subjects
{
    public class SubjectResponse
    {
        public Guid SubjectId { get; set; } // SỬA: Đổi từ string sang Guid
        public string Name { get; set; }
    }
}
