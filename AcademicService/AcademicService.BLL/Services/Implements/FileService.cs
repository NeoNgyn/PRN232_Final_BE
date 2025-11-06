using AcademicService.BLL.Services.Interfaces;

namespace AcademicService.BLL.Services.Implements;

public class FileService : IFileService
{
    public string GetFileUrl(string filePath)
    {
        // Simple file URL generation
        return filePath;
    }
}
