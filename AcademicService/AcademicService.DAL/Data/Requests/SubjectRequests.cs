namespace AcademicService.DAL.Data.Requests;

public class CreateSubjectRequest
{
    public string SubjectCode { get; set; } = string.Empty;
    public string SubjectName { get; set; } = string.Empty;
}

public class UpdateSubjectRequest
{
    public string SubjectCode { get; set; } = string.Empty;
    public string SubjectName { get; set; } = string.Empty;
}
