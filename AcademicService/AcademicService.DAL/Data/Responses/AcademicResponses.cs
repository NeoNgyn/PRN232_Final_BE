namespace AcademicService.DAL.Data.Responses;

public class ExamResponse
{
    public Guid ExamId { get; set; }
    public Guid SubjectId { get; set; }
    public Guid SemesterId { get; set; }
    public string ExamName { get; set; } = string.Empty;
    public string ExamType { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class SemesterResponse
{
    public Guid SemesterId { get; set; }
    public string SemesterCode { get; set; } = string.Empty;
    public string? SemesterName { get; set; }
}

public class SubjectResponse
{
    public Guid SubjectId { get; set; }
    public string SubjectCode { get; set; } = string.Empty;
    public string SubjectName { get; set; } = string.Empty;
}

public class StudentResponse
{
    public string StudentId { get; set; }
    public string FullName { get; set; } = string.Empty;
}
