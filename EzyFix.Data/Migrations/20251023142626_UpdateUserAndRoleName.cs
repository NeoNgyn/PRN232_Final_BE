using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EzyFix.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserAndRoleName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "lecturer_id",
                table: "LecturerSubject",
                newName: "user_id");

            migrationBuilder.RenameIndex(
                name: "IX_LecturerSubject_lecturer_id",
                table: "LecturerSubject",
                newName: "IX_LecturerSubject_user_id");

            migrationBuilder.RenameColumn(
                name: "lecturer_id",
                table: "GradingResult",
                newName: "user_id");

            migrationBuilder.RenameIndex(
                name: "IX_GradingResult_lecturer_id",
                table: "GradingResult",
                newName: "IX_GradingResult_user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "LecturerSubject",
                newName: "lecturer_id");

            migrationBuilder.RenameIndex(
                name: "IX_LecturerSubject_user_id",
                table: "LecturerSubject",
                newName: "IX_LecturerSubject_lecturer_id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "GradingResult",
                newName: "lecturer_id");

            migrationBuilder.RenameIndex(
                name: "IX_GradingResult_user_id",
                table: "GradingResult",
                newName: "IX_GradingResult_lecturer_id");
        }
    }
}
