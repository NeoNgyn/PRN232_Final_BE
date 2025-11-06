namespace AcademicService.DAL.Data.Requests;

public class CreateStudentRequest
{
    public string StudentMSSV { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
}

public class UpdateStudentRequest
{
    public string StudentMSSV { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
}
