using AcademicService.BLL.Services.Interfaces;
using AcademicService.DAL.Data.Requests;
using AcademicService.DAL.Data.Requests.Criteria;
using ClosedXML.Excel;
using System.Net.Http;
using System.Text.Json;

namespace AcademicService.BLL.Services.Implements
{
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

        public async Task<IEnumerable<CreateCriteriaRequest>> ReadCriteriasFromExcelAsync(string filePath, Guid examId)
        {
            var criterias = new List<CreateCriteriaRequest>();

            using (var workbook = new XLWorkbook(filePath))
            {
                var worksheet = workbook.Worksheet(1);
                var rows = worksheet.RowsUsed().Skip(1); // Bỏ dòng header

                foreach (var row in rows)
                {
                    try
                    {
                        var order = row.Cell(1).GetValue<int>();             // Cột "Order"
                        var name = row.Cell(2).GetString().Trim();           // Cột "Criteria"
                        var score = row.Cell(3).GetValue<decimal>();         // Cột "Score"

                        if (string.IsNullOrEmpty(name))
                            continue;

                        criterias.Add(new CreateCriteriaRequest
                        {
                            ExamId = examId,
                            SortOrder = order,
                            CriteriaName = name,
                            MaxScore = score
                        });
                    }
                    catch
                    {
                        // Bỏ qua dòng lỗi, tránh crash khi gặp giá trị trống
                        continue;
                    }
                }
            }

            await Task.CompletedTask;
            return criterias;
        }
    }
}
