namespace EzyFix.DAL.Data.Responses.Auth;

public class AuthResultResponse
{
    public bool Succeeded { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? Token { get; set; }
}