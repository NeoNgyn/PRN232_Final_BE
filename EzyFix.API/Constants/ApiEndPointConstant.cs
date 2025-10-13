namespace EzyFix.API.Constants
{
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
        }

        public static class Claim
        {
            public const string ClaimsEndpoint = ApiEndpoint + "/claims";
            public const string ClaimEndpointById = ClaimsEndpoint + "/{id}";
            public const string UpdateClaimEndpoint = ClaimEndpointById + "/update";
            public const string CancelClaimEndpoint = ClaimsEndpoint + "/{claimId}" + "/cancel";
            public const string RejectClaimEndpoint = ClaimEndpointById + "/reject";
            public const string ApproveClaimEndpoint = ClaimEndpointById + "/approve";
            public const string DownloadClaimEndpoint = ClaimsEndpoint + "/download";
            public const string PaidClaimEndpoint = ClaimEndpointById + "/paid";
            public const string ReturnClaimEndpoint = ClaimEndpointById + "/return";
            public const string SubmitClaimEndpoint = ClaimEndpointById + "/submit";
        }

        public static class Email
        {
            public const string EmailEndpoint = ApiEndpoint + "/email";
            public const string SendEmail = EmailEndpoint + "/send";
            public const string SendOtp = EmailEndpoint + "/send-otp";
        }
        //public static class Otp
        //{
        //    public const string OtpEndpoint = ApiEndpoint + "/otp";
        //    public const string ValidateOtp = OtpEndpoint + "/validate";
        //}
        public static class Projects
        {
            public const string ProjectsEndpoint = ApiEndpoint + "/projects";
            public const string ProjectEndpointById = ProjectsEndpoint + "/{id}";
            public const string UpdateProjectEndpoint = ProjectEndpointById + "/update";
            public const string DeleteProjectEndpoint = ProjectEndpointById + "/delete";
        }
        public static class Staffs
        {
            public const string StaffsEndpoint = ApiEndpoint + "/staffs";
            public const string StaffEndpointById = StaffsEndpoint + "/{id}";
            public const string UpdateStaffEndpoint = StaffEndpointById + "/update";
            public const string DeleteStaffEndpoint = StaffEndpointById + "/delete";
            public const string AssignStaffEndpoint = StaffEndpointById + "/assign";
            public const string RemoveStaffEndpoint = StaffEndpointById + "/remove";
        }
        public static class Cloudinary
        {
            public const string CloudinaryEndpoint = ApiEndpoint + "/cloudinary";
            public const string UploadImage = CloudinaryEndpoint + "/upload";
            public const string UploadFile = CloudinaryEndpoint + "/upload-file";
        }

        public static class Payment
        {
            public const string PaymentControllerBase = ApiEndpoint + "/payment";
            public const string CreatePaymentUrl = PaymentControllerBase + "/create-payment-url";
            public const string PaymentCallback = PaymentControllerBase + "/payment-callback";
        }

        public static class Remnider
        {
            public const string ReminderEndpoint = ApiEndpoint + "/send-reminder";
        }

        public static class PendingReminder
        {
            public const string PendingReminderEndpoint = ApiEndpoint + "/pending-reminder";
        }

        public static class Categories
        {
            public const string CategoriesEndpoint = ApiEndpoint + "/categories";
            public const string CategoryEndpointById = CategoriesEndpoint + "/{id}";
            public const string UpdateCategoryEndpoint = CategoryEndpointById + "/update";
            public const string DeleteCategoryEndpoint = CategoryEndpointById + "/delete";
        }

        public static class Services
        {
            public const string ServicesEndpoint = ApiEndpoint + "/sevices";
            public const string ServiceEndpointById = ServicesEndpoint + "/{id}";
            public const string UpdateServiceEndpoint = ServiceEndpointById + "/update";
            public const string DeleteServiceEndpoint = ServiceEndpointById + "/delete";
        }

        public static class ServiceRequests
        {
            public const string ServiceRequestsEndpoint = ApiEndpoint + "/seviceRequests";
            //public const string ServiceRequestsEndpointByService = ServiceRequestsEndpoint + "/{serviceId}";
            public const string ServiceRequestEndpointById = ServiceRequestsEndpoint + "/{id}";
            public const string UpdateServiceRequestEndpoint = ServiceRequestEndpointById + "/update";
            public const string UpdateServiceRequestStatusEndpoint = ServiceRequestEndpointById + "/status/update";
            public const string DeleteServiceRequestEndpoint = ServiceRequestEndpointById + "/delete";
        }

        public static class Medias
        {
            public const string MediasEndpoint = ApiEndpoint + "/media";
            public const string MediaEndpointById = MediasEndpoint + "/{id}";
            public const string UpdateMediaEndpoint = MediaEndpointById + "/update";
            public const string DeleteMediaEndpoint = MediaEndpointById + "/delete";
        }

        public static class Semesters
        {
            public const string SemestersEndpoint = "api/semesters";
            public const string SemesterEndpointById = "api/semesters/{id}";
            public const string UpdateSemesterEndpoint = "api/semesters/{id}";
            public const string DeleteSemesterEndpoint = "api/semesters/delete/{id}";
        }

        public static class Subjects
        {
            private const string Prefix = "api/subjects";

            public const string SubjectsEndpoint = $"{Prefix}";
            public const string SubjectEndpointById = $"{Prefix}/{{id}}";
            public const string UpdateSubjectEndpoint = $"{Prefix}/update/{{id}}";
            public const string DeleteSubjectEndpoint = $"{Prefix}/delete/{{id}}";
        }

        public static class Keywords
        {
            private const string Prefix = "api/keywords";

            public const string KeywordsEndpoint = $"{Prefix}";
            public const string KeywordEndpointById = $"{Prefix}/{{id}}";
            public const string UpdateKeywordEndpoint = $"{Prefix}/update/{{id}}";
            public const string DeleteKeywordEndpoint = $"{Prefix}/delete/{{id}}";
        }
    }
}
