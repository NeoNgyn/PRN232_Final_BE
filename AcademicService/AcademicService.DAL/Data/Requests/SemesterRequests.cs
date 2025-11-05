namespace AcademicService.DAL.Data.Requests;

public class CreateSemesterRequest
{
    public string SemesterCode { get; set; } = string.Empty;
    public string? SemesterName { get; set; }
}

public class UpdateSemesterRequest
{
    public string SemesterCode { get; set; } = string.Empty;
    public string? SemesterName { get; set; }
}
