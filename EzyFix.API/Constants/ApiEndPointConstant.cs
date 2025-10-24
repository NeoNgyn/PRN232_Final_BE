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

        public static class ScoreColumns
        {
            private const string Prefix = "api/score-columns";

            public const string ScoreColumnsEndpoint = $"{Prefix}";
            public const string ScoreColumnEndpointById = $"{Prefix}/{{id}}";
            public const string UpdateScoreColumnEndpoint = $"{Prefix}/update/{{id}}";
            public const string DeleteScoreColumnEndpoint = $"{Prefix}/delete/{{id}}";
        }

        public static class ExamGradingCriteria
        {
            private const string Prefix = "api/exam-grading-criteria";

            public const string ExamGradingCriteriaEndpoint = $"{Prefix}";
            public const string ExamGradingCriterionEndpointById = $"{Prefix}/{{id}}";
            public const string UpdateExamGradingCriterionEndpoint = $"{Prefix}/update/{{id}}";
            public const string DeleteExamGradingCriterionEndpoint = $"{Prefix}/delete/{{id}}";
        }

        public static class Students
        {
            private const string Prefix = "api/students";

            public const string StudentsEndpoint = $"{Prefix}";
            public const string StudentEndpointById = $"{Prefix}/{{id}}";
            public const string UpdateStudentEndpoint = $"{Prefix}/update/{{id}}";
            public const string DeleteStudentEndpoint = $"{Prefix}/delete/{{id}}";
        }

        public static class GradingDetails
        {
            private const string Prefix = "api/grading-details";

            public const string GradingDetailsEndpoint = $"{Prefix}";
            public const string GradingDetailEndpointById = $"{Prefix}/{{id}}";
            public const string UpdateGradingDetailEndpoint = $"{Prefix}/update/{{id}}";
            public const string DeleteGradingDetailEndpoint = $"{Prefix}/delete/{{id}}";
            public const string GradingDetailsByScoreEndpoint = $"{Prefix}/score/{{scoreId}}";
            public const string GradingDetailsByColumnEndpoint = $"{Prefix}/column/{{columnId}}";
        }

        public static class LecturerSubjects
        {
            private const string Prefix = "api/lecturer-subjects";

            public const string LecturerSubjectsEndpoint = $"{Prefix}";
            public const string LecturerSubjectEndpointById = $"{Prefix}/{{id}}";
            public const string UpdateLecturerSubjectEndpoint = $"{Prefix}/update/{{id}}";
            public const string DeleteLecturerSubjectEndpoint = $"{Prefix}/delete/{{id}}";
            public const string LecturerSubjectsByLecturerEndpoint = $"{Prefix}/lecturer/{{lecturerId}}";
            public const string LecturerSubjectsBySubjectEndpoint = $"{Prefix}/subject/{{subjectId}}";
            public const string LecturerSubjectsBySemesterEndpoint = $"{Prefix}/semester/{{semesterId}}";
        }

        public static class Assignments
        {
            private const string Prefix = "api/assignments";

            public const string AssignmentsEndpoint = $"{Prefix}";
            public const string AssignmentEndpointById = $"{Prefix}/{{id}}";
            public const string UpdateAssignmentEndpoint = $"{Prefix}/update/{{id}}";
            public const string DeleteAssignmentEndpoint = $"{Prefix}/delete/{{id}}";
            public const string AssignmentsByStudentEndpoint = $"{Prefix}/student/{{studentId}}";
            public const string AssignmentsByExamEndpoint = $"{Prefix}/exam/{{examId}}";
        }

        public static class Files
        {
            private const string Prefix = "api/files";
            public const string FilesEndpoint = $"{Prefix}";
            public const string FileEndpointById = $"{Prefix}/{{id}}";
            public const string UpdateFileEndpoint = $"{Prefix}/update/{{id}}";
            public const string DeleteFileEndpoint = $"{Prefix}/delete/{{id}}";
        }

        public static class Exams
        {
            private const string Prefix = "api/exams";

            public const string ExamsEndpoint = $"{Prefix}";
            public const string ExamEndpointById = $"{Prefix}/{{id}}";
            public const string CreateExamEndpoint = $"{Prefix}";
            public const string UpdateExamEndpoint = $"{Prefix}/update/{{id}}";
            public const string DeleteExamEndpoint = $"{Prefix}/delete/{{id}}";

            public const string UploadExamFile = $"{Prefix}/{{id}}/upload-file";
            public const string DownloadExamFile = $"{Prefix}/{{id}}/download";
            public const string ExtractedPathEndpoint = $"{Prefix}/{{id}}/extracted";
        }



    }
}
