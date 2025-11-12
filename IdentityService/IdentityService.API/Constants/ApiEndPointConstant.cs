namespace IdentityService.API.Constants
{
    public class ApiEndPointConstant
    {
        public const string RootEndpoint = "/api";
        public const string ApiVersion = "/v1";
        public const string ApiEndpoint = RootEndpoint + ApiVersion;

        public static class Auth
        {
            public const string AuthEndpoint = ApiEndpoint + "/auth";
            public const string GoogleLoginEndpoint = AuthEndpoint + "/google-login";
            public const string RefreshTokenEndpoint = AuthEndpoint + "/refresh-token";
            public const string DeleteRefreshTokenEndpoint = AuthEndpoint + "/delete-refresh-token";
        }

        public static class Roles
        {
            public const string RolesEndpoint = ApiEndpoint + "/roles";
            public const string RoleEndpointById = RolesEndpoint + "/{id}";
            public const string UpdateRoleEndpoint = RoleEndpointById;
            public const string DeleteRoleEndpoint = RoleEndpointById;
        }
        
        public static class Users
        {
            public const string UsersEndpoint =  ApiEndpoint + "/users";
            public const string UserEndpointById = UsersEndpoint + "/{id}";
            public const string UpdateUserEndpoint = UserEndpointById + "/update";
            public const string DeleteUserEndpoint = UserEndpointById + "/delete";
        }
    }
}
