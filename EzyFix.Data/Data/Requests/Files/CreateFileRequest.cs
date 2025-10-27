using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzyFix.DAL.Data.Requests.Files
{
    public class CreateFileRequest
    {
        public string FileName { get; set; }
        public string FileType { get; set; }
        public byte[] FileContent { get; set; }
        public Guid UploadedByUserId { get; set; }
    }
}
