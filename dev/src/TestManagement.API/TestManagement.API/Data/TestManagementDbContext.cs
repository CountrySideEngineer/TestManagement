using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using TestManagement.API.Models;

namespace TestManagement.API.Data
{
    public class TestManagementDbContext : DbContext
    {
        public TestManagementDbContext(DbContextOptions<TestManagementDbContext> options) : base(options) { }

        public DbSet<TestCase> TestCases { get; set; }
        public DbSet<TestLevel> TestLevels { get; set; }
        public DbSet<TestResult> TestResults { get; set; }
        public DbSet<TestRun> TestRuns { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TestLevel>()
                .HasMany(_ => _.TestCases)
                .WithOne(_ => _.TestLevel)
                .HasForeignKey(_ => _.TestLevelId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TestCase>()
                .HasMany(_ => _.Results)
                .WithOne(_ => _.TestCase)
                .HasForeignKey(_ => _.TestCaseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TestRun>()
                .HasMany(_ => _.TestResults)
                .WithOne(_ => _.TestRun)
                .HasForeignKey(_ => _.TestRunId)
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
    }
}
