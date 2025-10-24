using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EzyFix.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddLecturerCodeToLecturer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LecturerCode",
                table: "Lecturer",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LecturerCode",
                table: "Lecturer");
        }
    }
}
