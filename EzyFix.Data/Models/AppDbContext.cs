#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EzyFix.DAL.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    // --- DbSet MỚI ---
    public virtual DbSet<Criteria> Criteria { get; set; }
    public virtual DbSet<Exam> Exams { get; set; }
    public virtual DbSet<Grade> Grades { get; set; }
    public virtual DbSet<RefreshTokens> RefreshTokens { get; set; }
    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<Semester> Semesters { get; set; }
    public virtual DbSet<Student> Students { get; set; } // Sử dụng Student.cs đã sửa
    public virtual DbSet<Subject> Subjects { get; set; }
    public virtual DbSet<Submission> Submissions { get; set; }
    public virtual DbSet<TeacherAssignment> TeacherAssignments { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Violation> Violations { get; set; }

    // --- CÁC DbSet CŨ ĐÃ BỊ XÓA ---
    // (Assignments, ExamGradingCriteria, ExamKeywords, GradingDetails, 
    // GradingResults, Keywords, LecturerSubjects, ScoreColumns đã bị xóa)

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Cấu hình lại từ đầu cho các entity mới

        // --- User ---
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users"); // Đặt tên bảng rõ ràng
            entity.HasKey(e => e.UserId);

            // Đặt giá trị mặc định cho Guid PK trên Supabase/PostgreSQL
            entity.Property(e => e.UserId).HasDefaultValueSql("gen_random_uuid()");

            entity.HasIndex(e => e.Email).IsUnique(); // Đảm bảo Email là duy nhất

            // Quan hệ User -> Role (1 User có 1 Role)
            entity.HasOne(d => d.Role)
                  .WithMany(p => p.Users)
                  .HasForeignKey(d => d.RoleId)
                  .OnDelete(DeleteBehavior.SetNull); // Nếu Role bị xóa, RoleId của User set về NULL
        });

        // --- Role ---
        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Roles");
            entity.HasKey(e => e.RoleId);
            entity.Property(e => e.RoleId).HasDefaultValueSql("gen_random_uuid()");
            entity.HasIndex(e => e.RoleName).IsUnique(); // Tên vai trò là duy nhất
        });

        // --- Student ---
        modelBuilder.Entity<Student>(entity =>
        {
            // (Đã có [Table("Students")] và [Key] trong entity)
            entity.Property(e => e.StudentId).HasDefaultValueSql("gen_random_uuid()");
            entity.HasIndex(e => e.StudentMSSV).IsUnique(); // MSSV là duy nhất
        });

        // --- Subject ---
        modelBuilder.Entity<Subject>(entity =>
        {
            // (Đã có [Table("Subjects")] và [Key] trong entity)
            entity.Property(e => e.SubjectId).HasDefaultValueSql("gen_random_uuid()");
            entity.HasIndex(e => e.SubjectCode).IsUnique(); // Mã môn học là duy nhất
        });

        // --- Semester ---
        modelBuilder.Entity<Semester>(entity =>
        {
            // (Đã có [Table("Semesters")] và [Key] trong entity)
            entity.Property(e => e.SemesterId).HasDefaultValueSql("gen_random_uuid()");
            entity.HasIndex(e => e.SemesterCode).IsUnique(); // Mã học kỳ là duy nhất
        });

        // --- Exam ---
        modelBuilder.Entity<Exam>(entity =>
        {
            // (Đã có [Table("Exams")] và [Key] trong entity)
            entity.Property(e => e.ExamId).HasDefaultValueSql("gen_random_uuid()");

            // Quan hệ Exam -> Subject (1 Exam thuộc 1 Subject)
            entity.HasOne(d => d.Subject)
                  .WithMany(p => p.Exams)
                  .HasForeignKey(d => d.SubjectId)
                  .OnDelete(DeleteBehavior.Restrict); // Không cho xóa Subject nếu còn Exam

            // Quan hệ Exam -> Semester (1 Exam thuộc 1 Semester)
            entity.HasOne(d => d.Semester)
                  .WithMany(p => p.Exams)
                  .HasForeignKey(d => d.SemesterId)
                  .OnDelete(DeleteBehavior.Restrict); // Không cho xóa Semester nếu còn Exam
        });

        // --- TeacherAssignment ---
        modelBuilder.Entity<TeacherAssignment>(entity =>
        {
            // (Đã có [Table("TeacherAssignments")] và [Key] trong entity)
            entity.Property(e => e.AssignmentId).HasDefaultValueSql("gen_random_uuid()");

            // Quan hệ -> User (Teacher)
            entity.HasOne(d => d.Teacher)
                  .WithMany(p => p.TeacherAssignments)
                  .HasForeignKey(d => d.UserID_Teacher)
                  .OnDelete(DeleteBehavior.Restrict); // Không cho xóa User nếu còn được gán

            // Quan hệ -> Exam
            entity.HasOne(d => d.Exam)
                  .WithMany(p => p.TeacherAssignments)
                  .HasForeignKey(d => d.ExamId)
                  .OnDelete(DeleteBehavior.Restrict); // Không cho xóa Exam nếu còn gán
        });

        // --- Criteria ---
        modelBuilder.Entity<Criteria>(entity =>
        {
            // (Đã có [Table("Criteria")] và [Key] trong entity)
            entity.Property(e => e.CriteriaId).HasDefaultValueSql("gen_random_uuid()");

            // Quan hệ -> Exam
            entity.HasOne(d => d.Exam)
                  .WithMany(p => p.Criteria)
                  .HasForeignKey(d => d.ExamId)
                  .OnDelete(DeleteBehavior.Cascade); // Nếu xóa Exam, xóa hết Criteria
        });

        // --- Submission ---
        modelBuilder.Entity<Submission>(entity =>
        {
            // (Đã có [Table("Submissions")] và [Key] trong entity)
            entity.Property(e => e.SubmissionId).HasDefaultValueSql("gen_random_uuid()");

            // Quan hệ -> Exam
            entity.HasOne(d => d.Exam)
                  .WithMany(p => p.Submissions)
                  .HasForeignKey(d => d.ExamId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Quan hệ -> Student
            entity.HasOne(d => d.Student)
                  .WithMany(p => p.Submissions)
                  .HasForeignKey(d => d.StudentId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Quan hệ -> User (UploadedBy)
            entity.HasOne(d => d.UploadedBy)
                  .WithMany(p => p.Submissions)
                  .HasForeignKey(d => d.UploadedBy_UserID)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // --- Grade ---
        modelBuilder.Entity<Grade>(entity =>
        {
            // (Đã có [Table("Grades")] và [Key] trong entity)
            entity.Property(e => e.GradeId).HasDefaultValueSql("gen_random_uuid()");

            // Quan hệ -> Submission
            entity.HasOne(d => d.Submission)
                  .WithMany(p => p.Grades)
                  .HasForeignKey(d => d.SubmissionId)
                  .OnDelete(DeleteBehavior.Cascade); // Nếu xóa Submission, xóa hết Grade

            // Quan hệ -> Criteria
            entity.HasOne(d => d.Criteria)
                  .WithMany(p => p.Grades)
                  .HasForeignKey(d => d.CriteriaId)
                  .OnDelete(DeleteBehavior.Restrict); // Không cho xóa Criteria nếu còn Grade
        });

        // --- Violation ---
        modelBuilder.Entity<Violation>(entity =>
        {
            // (Đã có [Table("Violations")] và [Key] trong entity)
            entity.Property(e => e.ViolationId).HasDefaultValueSql("gen_random_uuid()");

            // Quan hệ -> Submission
            entity.HasOne(d => d.Submission)
                  .WithMany(p => p.Violations)
                  .HasForeignKey(d => d.SubmissionId)
                  .OnDelete(DeleteBehavior.Cascade); // Nếu xóa Submission, xóa hết Violation

            // Quan hệ -> User (DetectedBy)
            entity.HasOne(d => d.DetectedBy)
                  .WithMany(p => p.Violations)
                  .HasForeignKey(d => d.DetectedBy_UserID)
                  .OnDelete(DeleteBehavior.SetNull); // Nếu User bị xóa, set ID về NULL
        });

        // --- RefreshTokens ---
        modelBuilder.Entity<RefreshTokens>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");

            // Quan hệ -> User
            entity.HasOne(d => d.User)
                  .WithMany() // Không có collection điều hướng ngược lại trong User
                  .HasForeignKey(d => d.UserId)
                  .OnDelete(DeleteBehavior.Cascade); // Nếu User bị xóa, xóa hết token
        });


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}