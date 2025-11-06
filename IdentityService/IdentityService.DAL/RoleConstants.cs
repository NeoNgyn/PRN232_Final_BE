using System;

namespace IdentityService.DAL
{
    public static class RoleConstants
    {
        // Default role IDs - should match your database
        public static readonly Guid AdminRoleId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        public static readonly Guid LecturerRoleId = Guid.Parse("22222222-2222-2222-2222-222222222222");
        public static readonly Guid DefaultUserRoleId = Guid.Parse("33333333-3333-3333-3333-333333333333");

        // Role names
        public const string AdminRole = "Admin";
        public const string LecturerRole = "Lecturer";
        public const string StudentRole = "Student";
    }
}
