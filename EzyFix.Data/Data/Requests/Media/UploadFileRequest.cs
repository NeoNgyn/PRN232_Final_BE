using Microsoft.AspNetCore.Http;

namespace EzyFix.DAL.Data.Requests.Media
{
    public class UploadFileRequest
    {
        public IFormFile File { get; set; }
    }
}
