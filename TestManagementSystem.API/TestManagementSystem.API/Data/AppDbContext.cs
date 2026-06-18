using Microsoft.EntityFrameworkCore;
using TestManagementSystem.API.Models;

namespace TestManagementSystem.API.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectMember> ProjectMembers { get; set; }
        public DbSet<TestCase> TestCases { get; set; }
        public DbSet<TestRun> TestRuns { get; set; }
        public DbSet<TestRunResult> TestRunResults { get; set; }
        public DbSet<Bug> Bugs { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Comment nullable FK
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Bug)
                .WithMany(b => b.Comments)
                .HasForeignKey(c => c.BugId)
                .IsRequired(false);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.TestCase)
                .WithMany(tc => tc.Comments)
                .HasForeignKey(c => c.TestCaseId)
                .IsRequired(false);

            // Attachment nullable FK
            modelBuilder.Entity<Attachment>()
                .HasOne(a => a.Bug)
                .WithMany(b => b.Attachments)
                .HasForeignKey(a => a.BugId)
                .IsRequired(false);

            modelBuilder.Entity<Attachment>()
                .HasOne(a => a.TestCase)
                .WithMany(tc => tc.Attachments)
                .HasForeignKey(a => a.TestCaseId)
                .IsRequired(false);
        }
    }
}
