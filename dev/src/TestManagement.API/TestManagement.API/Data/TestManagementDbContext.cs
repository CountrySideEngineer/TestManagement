using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using TestManagement.API.Models;

namespace TestManagement.API.Data
{
    public class TestManagementDbContext : DbContext
    {
        public TestManagementDbContext(DbContextOptions<TestManagementDbContext> options) : base(options) { }

        public DbSet<TestCaseVersion> TestCaseVersions { get; set; }
        public DbSet<TestLevel> TestLevels { get; set; }
        public DbSet<TestResult> TestResults { get; set; }
        public DbSet<TestRun> TestRuns { get; set; }
        public DbSet<TestCase> TestCases { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureTestLevel(modelBuilder);
            ConfigureTestCase(modelBuilder);
            ConfigureTestCaseVersion(modelBuilder);
            ConfigureTestResult(modelBuilder);
            ConfigureTestRun(modelBuilder);
        }

        private void ConfigureTestLevel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TestLevel>()
                .HasMany(_ => _.TestCases)
                .WithOne(_ => _.TestLevel)
                .HasForeignKey(_ => _.TestLevelId)
                .OnDelete(DeleteBehavior.Restrict);


            // Add seed TestLevel class (Unit, Integration, System, Acceptance)
            var seedDate = new DateTime(2025, 11, 21, 0, 0, 0, DateTimeKind.Utc);
            modelBuilder.Entity<TestLevel>(_ =>
            {
                _.HasData(
                    new TestLevel { Id = 1, Name = "Unit", Description = "Unit Level Testing", CreatedAt = seedDate, UpdatedAt = seedDate },
                    new TestLevel { Id = 2, Name = "Integration", Description = "Integration Level Testing", CreatedAt = seedDate, UpdatedAt = seedDate },
                    new TestLevel { Id = 3, Name = "System", Description = "System Level Testing", CreatedAt = seedDate, UpdatedAt = seedDate },
                    new TestLevel { Id = 4, Name = "Acceptance", Description = "Acceptance Level Testing", CreatedAt = seedDate, UpdatedAt = seedDate });
            });
        }

        private void ConfigureTestResult(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TestResult>()
                .HasOne(_ => _.TestCaseVersion)
                .WithMany()
                .HasForeignKey(_ => _.TestCaseVersionId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private void ConfigureTestRun(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TestRun>()
                .HasMany(_ => _.TestResults)
                .WithOne(_ => _.TestRun)
                .HasForeignKey(_ => _.TestRunId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private void ConfigureTestCase(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TestCase>()
                .HasMany(_ => _.Versions)
                .WithOne(_ => _.TestCase)
                .HasForeignKey(_ => _.TestCaseId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private void ConfigureTestCaseVersion(ModelBuilder builder)
        {
            var entity = builder.Entity<TestCaseVersion>();

            entity.HasIndex(_ => new { _.TestCaseId, _.VersionNumber })
                .IsUnique();

            entity.HasIndex(_ => _.TestCaseId)
                .IsUnique();

            entity.HasIndex(_ => new { _.Title, _.Description, _.TestLevelId })
                .IsUnique();

            builder.Entity<TestCaseVersion>()
                    .HasIndex(_ => new { _.Title, _.Description, _.TestLevelId })
                    .IsUnique();
        }
    }
}
