using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzyFix.DAL.Data.Responses.ExamKeyword
{
    public class ExamKeywordResponse
    {
        public Guid ExamKeywordId { get; set; }
        public Guid ExamId { get; set; }
        public Guid KeywordId { get; set; }

        // Related entities
        public ExamDetails Exam { get; set; }
        public KeywordDetails Keyword { get; set; }

        public class ExamDetails
        {
            public Guid ExamId { get; set; }
            public string Title { get; set; }
        }

        public class KeywordDetails
        {
            public Guid KeywordId { get; set; }
            public string Word { get; set; }
        }
    }
}
