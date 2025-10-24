using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EzyFix.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserAndRoleTablesFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__GradingRe__lectu__59FA5E80",
                table: "GradingResult");

            migrationBuilder.DropForeignKey(
                name: "FK__LecturerS__lectu__5070F446",
                table: "LecturerSubject");

            migrationBuilder.DropTable(
                name: "Lecturer");

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    role_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    role_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Role__760965CC", x => x.role_id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    password = table.Column<string>(type: "character varying(255)", unicode: false, maxLength: 255, nullable: false),
                    email = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: false),
                    LecturerCode = table.Column<string>(type: "text", nullable: true),
                    email_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: true),
                    createdAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    role_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_User_Role",
                        column: x => x.role_id,
                        principalTable: "Role",
                        principalColumn: "role_id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_email",
                table: "User",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_role_id",
                table: "User",
                column: "role_id");

            migrationBuilder.AddForeignKey(
                name: "FK__GradingRe__lectu__59FA5E80",
                table: "GradingResult",
                column: "lecturer_id",
                principalTable: "User",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK__LecturerS__lectu__5070F446",
                table: "LecturerSubject",
                column: "lecturer_id",
                principalTable: "User",
                principalColumn: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__GradingRe__lectu__59FA5E80",
                table: "GradingResult");

            migrationBuilder.DropForeignKey(
                name: "FK__LecturerS__lectu__5070F446",
                table: "LecturerSubject");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.CreateTable(
                name: "Lecturer",
                columns: table => new
                {
                    lecturer_id = table.Column<Guid>(type: "uuid", unicode: false, maxLength: 10, nullable: false),
                    createdAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    email = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: false),
                    email_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: true),
                    LecturerCode = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    password = table.Column<string>(type: "character varying(255)", unicode: false, maxLength: 255, nullable: false),
                    updatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Lecturer__D4D1DAB1CC25A84F", x => x.lecturer_id);
                });

            migrationBuilder.CreateIndex(
                name: "UQ__Lecturer__AB6E61643BDB4DEC",
                table: "Lecturer",
                column: "email",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK__GradingRe__lectu__59FA5E80",
                table: "GradingResult",
                column: "lecturer_id",
                principalTable: "Lecturer",
                principalColumn: "lecturer_id");

            migrationBuilder.AddForeignKey(
                name: "FK__LecturerS__lectu__5070F446",
                table: "LecturerSubject",
                column: "lecturer_id",
                principalTable: "Lecturer",
                principalColumn: "lecturer_id");
        }
    }
}
