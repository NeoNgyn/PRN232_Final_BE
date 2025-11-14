using System;

namespace IdentityService.DAL
{
    public static class RoleConstants
    {
        // Default role IDs - should match your database
        public static readonly Guid AdminRoleId = Guid.Parse("a2eaee3a-8e34-400e-a2d5-1c0ac26e3124");
        public static readonly Guid ManagerRoleId = Guid.Parse("db45356f-1f54-432c-877a-965aa64e593d");
        public static readonly Guid ModeratorRoleId = Guid.Parse("b34ee72e-7d5e-4cc1-aaa3-27d53d86f497");
        public static readonly Guid ExaminerRoleId = Guid.Parse("907a702d-5b01-4072-84b4-72de619568f9");
        public static readonly Guid DefaultUserRoleId = Guid.Parse("33333333-3333-3333-3333-333333333333");

        // Role names
        public const string AdminRole = "Admin";
        public const string LecturerRole = "Lecturer";
        public const string StudentRole = "Student";
    }
}
