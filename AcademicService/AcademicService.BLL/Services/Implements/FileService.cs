using AcademicService.BLL.Services.Interfaces;
using AcademicService.DAL.Data.Requests;
using System.Net.Http;
using System.Text.Json;

namespace AcademicService.BLL.Services.Implements;

public class FileService : IFileService
{
    private readonly HttpClient _httpClient;

    public FileService()
    {
        _httpClient = new HttpClient();
    }

    public string GetFileUrl(string filePath)
    {
        // Simple file URL generation
        return filePath;
    }

    public async Task<IEnumerable<CreateStudentRequest>> ReadStudentsFromJsonAsync(string filePath)
    {
        string jsonContent;

        if (filePath.StartsWith("http", StringComparison.OrdinalIgnoreCase))
        {
            jsonContent = await _httpClient.GetStringAsync(filePath);
        }
        else
        {
            jsonContent = await File.ReadAllTextAsync(filePath);
        }

        var students = JsonSerializer.Deserialize<IEnumerable<CreateStudentRequest>>(jsonContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return students ?? new List<CreateStudentRequest>();
    }
}
