using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicService.DAL.Data.Requests.File
{
    public class ExtractRARRequest
    {
        public IFormFile RARFile { get; set; } 
        public Guid ExamId { get; set; } 
        public Guid ExaminerId { get; set; } 
    }
}
