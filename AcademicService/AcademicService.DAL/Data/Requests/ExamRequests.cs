namespace AcademicService.DAL.Data.Requests;

public class CreateExamRequest
{
    public Guid SubjectId { get; set; }
    public Guid SemesterId { get; set; }
    public string ExamName { get; set; } = string.Empty;
    public string ExamType { get; set; } = string.Empty;
    public string? ExamPassword { get; set; }
}

public class UpdateExamRequest
{
    public Guid SubjectId { get; set; }
    public Guid SemesterId { get; set; }
    public string ExamName { get; set; } = string.Empty;
    public string ExamType { get; set; } = string.Empty;
    public string? ExamPassword { get; set; }
}
