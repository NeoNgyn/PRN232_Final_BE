using Microsoft.AspNetCore.Http;

namespace EzyFix.DAL.Data.Requests.Media
{
    public class UploadImageRequest
    {
        public IFormFile File { get; set; }
    }
}
