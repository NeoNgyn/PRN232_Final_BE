using Microsoft.AspNetCore.Http;

namespace AcademicService.BLL.Services.Interfaces;

public interface ICloudinaryService
{
    Task<string> UploadFileAsync(IFormFile file, string folder);
    Task<bool> DeleteFileAsync(string publicId);
}
