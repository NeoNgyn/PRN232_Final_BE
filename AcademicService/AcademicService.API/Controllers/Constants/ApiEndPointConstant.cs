namespace AcademicService.API.Constants;

public class ApiEndPointConstant
{
    public const string RootEndpoint = "/api";
    public const string ApiVersion = "/v1";
    public const string ApiEndpoint = RootEndpoint + ApiVersion;

    public static class Navigator
    {
        public const string SideBarEndpoint = ApiEndpoint + "/sidebar";
    }
    public static class Auth
    {
        public const string AuthEndpoint = ApiEndpoint + "/auth";
        public const string LoginEndpoint = AuthEndpoint + "/login";
        public const string RegisterEndpoint = AuthEndpoint + "/register";
        public const string RefreshTokenEndpoint = AuthEndpoint + "/refresh-token";
        public const string DeleteRefreshTokenEndpoint = AuthEndpoint + "/delete-refresh-token";
        public const string LogoutEndpoint = AuthEndpoint + "/logout";
        public const string ForgotPasswordEndpoint = AuthEndpoint + "/forgot-password";
        public const string ChangePasswordEndpoint = AuthEndpoint + "/change-password";
        public const string VerifyAccountEndpoint = AuthEndpoint + "/verify";
    }

    public static class Tools
    {
        public const string ToolsEndpoint = ApiEndpoint + "/tools";
        public const string HashPasswordEndpoint = ToolsEndpoint + "/hash-password";
    }

    public static class Cloudinary
    {
        public const string CloudinaryEndpoint = ApiEndpoint + "/cloudinary";
        public const string UploadImage = CloudinaryEndpoint + "/upload";
        public const string UploadFile = CloudinaryEndpoint + "/upload-file";
    }

    public static class Submissions
    {
        public const string SubmissionsEndpoint = ApiEndpoint + "/submission";
        public const string SubmissionEndpointById = SubmissionsEndpoint + "/{id}";
        public const string QuerySubmissionEndpoint = SubmissionsEndpoint + "/query";
        public const string UpdateSubmissionEndpoint = SubmissionEndpointById + "/update";
        public const string DeleteSubmissionEndpoint = SubmissionEndpointById + "/delete";
    }

    public static class Criterias
    {
        public const string CriteriasEndpoint = ApiEndpoint + "/criteria";
        public const string CriteriaEndpointById = CriteriasEndpoint + "/{id}";
        public const string CriteriaEndpointByExamId = CriteriasEndpoint + "/{examId}";
        public const string QueryCriteriaEndpoint = CriteriasEndpoint + "/query";
        public const string UpdateCriteriaEndpoint = CriteriaEndpointById + "/update";
        public const string DeleteCriteriaEndpoint = CriteriaEndpointById + "/delete";
    }

    public static class Grades
    {
        public const string GradesEndpoint = ApiEndpoint + "/grade";
        public const string GradeEndpointById = GradesEndpoint + "/{id}";
        public const string QueryGradeEndpoint = GradesEndpoint + "/query";
        public const string UpdateGradeEndpoint = GradeEndpointById + "/update";
        public const string DeleteGradeEndpoint = GradeEndpointById + "/delete";
    }

    public static class Violations
    {
        public const string ViolationsEndpoint = ApiEndpoint + "/violation";
        public const string ViolationEndpointById = ViolationsEndpoint + "/{id}";
        public const string QueryViolationEndpoint = ViolationsEndpoint + "/query";
        public const string UpdateViolationEndpoint = ViolationEndpointById + "/update";
        public const string DeleteViolationEndpoint = ViolationEndpointById + "/delete";
    }
}
