using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EzyFix.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Keyword",
                columns: table => new
                {
                    keyword_id = table.Column<Guid>(type: "uuid", nullable: false),
                    word = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Keyword__03E8D7CFE5EF6E0C", x => x.keyword_id);
                });

            migrationBuilder.CreateTable(
                name: "Lecturer",
                columns: table => new
                {
                    lecturer_id = table.Column<Guid>(type: "uuid", unicode: false, maxLength: 10, nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    password = table.Column<string>(type: "character varying(255)", unicode: false, maxLength: 255, nullable: false),
                    email = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: false),
                    email_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: true),
                    createdAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Lecturer__D4D1DAB1CC25A84F", x => x.lecturer_id);
                });

            migrationBuilder.CreateTable(
                name: "ScoreColumn",
                columns: table => new
                {
                    column_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ScoreCol__E301851F01951393", x => x.column_id);
                });

            migrationBuilder.CreateTable(
                name: "Semester",
                columns: table => new
                {
                    semester_id = table.Column<Guid>(type: "uuid", unicode: false, maxLength: 20, nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    start_date = table.Column<DateOnly>(type: "date", nullable: true),
                    end_date = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Semester__CBC81B019CF2FE3B", x => x.semester_id);
                });

            migrationBuilder.CreateTable(
                name: "Student",
                columns: table => new
                {
                    student_id = table.Column<Guid>(type: "uuid", unicode: false, maxLength: 10, nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    email = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: false),
                    createdAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Student__2A33069A0275A9C7", x => x.student_id);
                });

            migrationBuilder.CreateTable(
                name: "Subject",
                columns: table => new
                {
                    subject_id = table.Column<Guid>(type: "uuid", unicode: false, maxLength: 10, nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Subject__5004F66049D8CD3E", x => x.subject_id);
                });

            migrationBuilder.CreateTable(
                name: "LecturerSubject",
                columns: table => new
                {
                    lecturer_subject_id = table.Column<Guid>(type: "uuid", unicode: false, maxLength: 20, nullable: false),
                    lecturer_id = table.Column<Guid>(type: "uuid", unicode: false, maxLength: 10, nullable: false),
                    subject_id = table.Column<Guid>(type: "uuid", unicode: false, maxLength: 10, nullable: false),
                    semester_id = table.Column<Guid>(type: "uuid", unicode: false, maxLength: 20, nullable: false),
                    is_principal = table.Column<bool>(type: "boolean", nullable: true),
                    assignedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Lecturer__E621407CF17AF786", x => x.lecturer_subject_id);
                    table.ForeignKey(
                        name: "FK__LecturerS__lectu__5070F446",
                        column: x => x.lecturer_id,
                        principalTable: "Lecturer",
                        principalColumn: "lecturer_id");
                    table.ForeignKey(
                        name: "FK__LecturerS__semes__52593CB8",
                        column: x => x.semester_id,
                        principalTable: "Semester",
                        principalColumn: "semester_id");
                    table.ForeignKey(
                        name: "FK__LecturerS__subje__5165187F",
                        column: x => x.subject_id,
                        principalTable: "Subject",
                        principalColumn: "subject_id");
                });

            migrationBuilder.CreateTable(
                name: "Exam",
                columns: table => new
                {
                    exam_id = table.Column<Guid>(type: "uuid", unicode: false, maxLength: 20, nullable: false),
                    title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    file_path = table.Column<string>(type: "character varying(255)", unicode: false, maxLength: 255, nullable: false),
                    extracted_path = table.Column<string>(type: "character varying(255)", unicode: false, maxLength: 255, nullable: true),
                    uploadedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lecturer_subject_id = table.Column<Guid>(type: "uuid", unicode: false, maxLength: 20, nullable: false),
                    createdAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Exam__9C8C7BE9527237D6", x => x.exam_id);
                    table.ForeignKey(
                        name: "FK__Exam__lecturer_s__534D60F1",
                        column: x => x.lecturer_subject_id,
                        principalTable: "LecturerSubject",
                        principalColumn: "lecturer_subject_id");
                });

            migrationBuilder.CreateTable(
                name: "Assignment",
                columns: table => new
                {
                    assignment_id = table.Column<Guid>(type: "uuid", unicode: false, maxLength: 20, nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    file_path = table.Column<string>(type: "character varying(255)", unicode: false, maxLength: 255, nullable: false),
                    submission_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deadline = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    status = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: true),
                    student_id = table.Column<Guid>(type: "uuid", unicode: false, maxLength: 10, nullable: false),
                    exam_id = table.Column<Guid>(type: "uuid", unicode: false, maxLength: 20, nullable: false),
                    createdAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Assignme__DA891814051AD56B", x => x.assignment_id);
                    table.ForeignKey(
                        name: "FK__Assignmen__exam___59063A47",
                        column: x => x.exam_id,
                        principalTable: "Exam",
                        principalColumn: "exam_id");
                    table.ForeignKey(
                        name: "FK__Assignmen__stude__5812160E",
                        column: x => x.student_id,
                        principalTable: "Student",
                        principalColumn: "student_id");
                });

            migrationBuilder.CreateTable(
                name: "ExamGradingCriteria",
                columns: table => new
                {
                    criteria_id = table.Column<Guid>(type: "uuid", nullable: false),
                    exam_id = table.Column<Guid>(type: "uuid", unicode: false, maxLength: 20, nullable: false),
                    column_id = table.Column<Guid>(type: "uuid", nullable: false),
                    max_mark = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ExamGrad__401F949D6CCF0428", x => x.criteria_id);
                    table.ForeignKey(
                        name: "FK__ExamGradi__colum__571DF1D5",
                        column: x => x.column_id,
                        principalTable: "ScoreColumn",
                        principalColumn: "column_id");
                    table.ForeignKey(
                        name: "FK__ExamGradi__exam___5629CD9C",
                        column: x => x.exam_id,
                        principalTable: "Exam",
                        principalColumn: "exam_id");
                });

            migrationBuilder.CreateTable(
                name: "ExamKeyword",
                columns: table => new
                {
                    examKeyword_id = table.Column<Guid>(type: "uuid", unicode: false, maxLength: 20, nullable: false),
                    exam_id = table.Column<Guid>(type: "uuid", unicode: false, maxLength: 20, nullable: false),
                    keyword_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ExamKeyw__F9DC44C9DA869D2D", x => x.examKeyword_id);
                    table.ForeignKey(
                        name: "FK__ExamKeywo__exam___5441852A",
                        column: x => x.exam_id,
                        principalTable: "Exam",
                        principalColumn: "exam_id");
                    table.ForeignKey(
                        name: "FK__ExamKeywo__keywo__5535A963",
                        column: x => x.keyword_id,
                        principalTable: "Keyword",
                        principalColumn: "keyword_id");
                });

            migrationBuilder.CreateTable(
                name: "GradingResult",
                columns: table => new
                {
                    result_id = table.Column<Guid>(type: "uuid", unicode: false, maxLength: 20, nullable: false),
                    total_mark = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    checkedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    note = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    lecturer_id = table.Column<Guid>(type: "uuid", unicode: false, maxLength: 10, nullable: false),
                    assignment_id = table.Column<Guid>(type: "uuid", unicode: false, maxLength: 20, nullable: false),
                    createdAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__GradingR__AFB3C316C9F23B79", x => x.result_id);
                    table.ForeignKey(
                        name: "FK__GradingRe__assig__5AEE82B9",
                        column: x => x.assignment_id,
                        principalTable: "Assignment",
                        principalColumn: "assignment_id");
                    table.ForeignKey(
                        name: "FK__GradingRe__lectu__59FA5E80",
                        column: x => x.lecturer_id,
                        principalTable: "Lecturer",
                        principalColumn: "lecturer_id");
                });

            migrationBuilder.CreateTable(
                name: "GradingDetail",
                columns: table => new
                {
                    detail_id = table.Column<Guid>(type: "uuid", nullable: false),
                    score_id = table.Column<Guid>(type: "uuid", unicode: false, maxLength: 20, nullable: false),
                    column_id = table.Column<Guid>(type: "uuid", nullable: false),
                    mark = table.Column<decimal>(type: "numeric(5,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__GradingD__38E9A224518CE962", x => x.detail_id);
                    table.ForeignKey(
                        name: "FK__GradingDe__colum__5CD6CB2B",
                        column: x => x.column_id,
                        principalTable: "ScoreColumn",
                        principalColumn: "column_id");
                    table.ForeignKey(
                        name: "FK__GradingDe__score__5BE2A6F2",
                        column: x => x.score_id,
                        principalTable: "GradingResult",
                        principalColumn: "result_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assignment_exam_id",
                table: "Assignment",
                column: "exam_id");

            migrationBuilder.CreateIndex(
                name: "IX_Assignment_student_id",
                table: "Assignment",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "IX_Exam_lecturer_subject_id",
                table: "Exam",
                column: "lecturer_subject_id");

            migrationBuilder.CreateIndex(
                name: "IX_ExamGradingCriteria_column_id",
                table: "ExamGradingCriteria",
                column: "column_id");

            migrationBuilder.CreateIndex(
                name: "IX_ExamGradingCriteria_exam_id",
                table: "ExamGradingCriteria",
                column: "exam_id");

            migrationBuilder.CreateIndex(
                name: "IX_ExamKeyword_exam_id",
                table: "ExamKeyword",
                column: "exam_id");

            migrationBuilder.CreateIndex(
                name: "IX_ExamKeyword_keyword_id",
                table: "ExamKeyword",
                column: "keyword_id");

            migrationBuilder.CreateIndex(
                name: "IX_GradingDetail_column_id",
                table: "GradingDetail",
                column: "column_id");

            migrationBuilder.CreateIndex(
                name: "IX_GradingDetail_score_id",
                table: "GradingDetail",
                column: "score_id");

            migrationBuilder.CreateIndex(
                name: "IX_GradingResult_assignment_id",
                table: "GradingResult",
                column: "assignment_id");

            migrationBuilder.CreateIndex(
                name: "IX_GradingResult_lecturer_id",
                table: "GradingResult",
                column: "lecturer_id");

            migrationBuilder.CreateIndex(
                name: "UQ__Lecturer__AB6E61643BDB4DEC",
                table: "Lecturer",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LecturerSubject_lecturer_id",
                table: "LecturerSubject",
                column: "lecturer_id");

            migrationBuilder.CreateIndex(
                name: "IX_LecturerSubject_semester_id",
                table: "LecturerSubject",
                column: "semester_id");

            migrationBuilder.CreateIndex(
                name: "IX_LecturerSubject_subject_id",
                table: "LecturerSubject",
                column: "subject_id");

            migrationBuilder.CreateIndex(
                name: "UQ__Student__AB6E6164E0EDF09D",
                table: "Student",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExamGradingCriteria");

            migrationBuilder.DropTable(
                name: "ExamKeyword");

            migrationBuilder.DropTable(
                name: "GradingDetail");

            migrationBuilder.DropTable(
                name: "Keyword");

            migrationBuilder.DropTable(
                name: "ScoreColumn");

            migrationBuilder.DropTable(
                name: "GradingResult");

            migrationBuilder.DropTable(
                name: "Assignment");

            migrationBuilder.DropTable(
                name: "Exam");

            migrationBuilder.DropTable(
                name: "Student");

            migrationBuilder.DropTable(
                name: "LecturerSubject");

            migrationBuilder.DropTable(
                name: "Lecturer");

            migrationBuilder.DropTable(
                name: "Semester");

            migrationBuilder.DropTable(
                name: "Subject");
        }
    }
}
