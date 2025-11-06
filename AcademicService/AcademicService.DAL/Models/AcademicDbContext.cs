using Microsoft.EntityFrameworkCore;

namespace AcademicService.DAL.Models;

public class AcademicDbContext : DbContext
{
    public AcademicDbContext(DbContextOptions<AcademicDbContext> options) : base(options)
    {
    }

    public virtual DbSet<Exam> Exams { get; set; }
    public virtual DbSet<Semester> Semesters { get; set; }
    public virtual DbSet<Subject> Subjects { get; set; }
    public virtual DbSet<Student> Students { get; set; }
    public virtual DbSet<Criteria> Criteria { get; set; }
    public virtual DbSet<Grade> Grades { get; set; }
    public virtual DbSet<Submission> Submissions { get; set; }
    public virtual DbSet<TeacherAssignment> TeacherAssignments { get; set; }
    public virtual DbSet<Violation> Violations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Subject
        modelBuilder.Entity<Subject>(entity =>
        {
            entity.Property(e => e.SubjectId).HasDefaultValueSql("gen_random_uuid()");
            entity.HasIndex(e => e.SubjectCode).IsUnique();
        });

        // Semester
        modelBuilder.Entity<Semester>(entity =>
        {
            entity.Property(e => e.SemesterId).HasDefaultValueSql("gen_random_uuid()");
            entity.HasIndex(e => e.SemesterCode).IsUnique();
        });

        // Exam
        modelBuilder.Entity<Exam>(entity =>
        {
            entity.Property(e => e.ExamId).HasDefaultValueSql("gen_random_uuid()");

            entity.HasOne(d => d.Subject)
                  .WithMany(p => p.Exams)
                  .HasForeignKey(d => d.SubjectId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(d => d.Semester)
                  .WithMany(p => p.Exams)
                  .HasForeignKey(d => d.SemesterId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // TeacherAssignment
        modelBuilder.Entity<TeacherAssignment>(entity =>
        {
            entity.Property(e => e.AssignmentId).HasDefaultValueSql("gen_random_uuid()");

            entity.HasOne(d => d.Exam)
                  .WithMany(p => p.TeacherAssignments)
                  .HasForeignKey(d => d.ExamId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Criteria
        modelBuilder.Entity<Criteria>(entity =>
        {
            entity.Property(e => e.CriteriaId).HasDefaultValueSql("gen_random_uuid()");

            entity.HasOne(d => d.Exam)
                  .WithMany(p => p.Criteria)
                  .HasForeignKey(d => d.ExamId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Student
        modelBuilder.Entity<Student>(entity =>
        {
            entity.Property(e => e.StudentId).HasDefaultValueSql("gen_random_uuid()");
            entity.HasIndex(e => e.StudentMSSV).IsUnique();
        });

        // Submission
        modelBuilder.Entity<Submission>(entity =>
        {
            entity.Property(e => e.SubmissionId).HasDefaultValueSql("gen_random_uuid()");

            entity.HasOne(d => d.Exam)
                  .WithMany(p => p.Submissions)
                  .HasForeignKey(d => d.ExamId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(d => d.Student)
                  .WithMany(p => p.Submissions)
                  .HasForeignKey(d => d.StudentId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Grade
        modelBuilder.Entity<Grade>(entity =>
        {
            entity.Property(e => e.GradeId).HasDefaultValueSql("gen_random_uuid()");

            entity.HasOne(d => d.Submission)
                  .WithMany(p => p.Grades)
                  .HasForeignKey(d => d.SubmissionId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.Criteria)
                  .WithMany(p => p.Grades)
                  .HasForeignKey(d => d.CriteriaId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Violation
        modelBuilder.Entity<Violation>(entity =>
        {
            entity.Property(e => e.ViolationId).HasDefaultValueSql("gen_random_uuid()");

            entity.HasOne(d => d.Submission)
                  .WithMany(p => p.Violations)
                  .HasForeignKey(d => d.SubmissionId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
