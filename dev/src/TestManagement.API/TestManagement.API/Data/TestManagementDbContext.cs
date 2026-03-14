using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System.Data.Common;
using System.Security.Cryptography.X509Certificates;
using TestManagement.API.Models;
using Environment = TestManagement.API.Models.Environment;

namespace TestManagement.API.Data
{
    public class TestManagementDbContext : DbContext
    {
        public TestManagementDbContext(DbContextOptions<TestManagementDbContext> options) : base(options) { }

        public DbSet<TestLevel> TestLevels { get; set; }

        public DbSet<TestCaseVersion> TestCaseVersions { get; set; }

        public DbSet<TestCase> TestCases { get; set; }

        public DbSet<TestResult> TestResults { get; set; }

        public DbSet<TestStatus> TestStatuses { get; set; }

        public DbSet<Environment> Environments { get; set; }

        public DbSet<TestExecutionItem> TestExecutionItems { get; set; }

        public DbSet<TestExecution> TestExecutions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureTestLevel(modelBuilder);
            ConfigureTestCaseVersion(modelBuilder);
            ConfigureTestCase(modelBuilder);
            ConfigureTestResult(modelBuilder);
            ConfigureTestStatus(modelBuilder);
            ConfigureEnvironment(modelBuilder);
        }

        private void ConfigureTestLevel(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<TestLevel>();

            entity.HasKey(_ => _.Id);

            entity.Property(_ => _.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(_ => _.DisplayName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(_ => _.Description)
                .HasMaxLength(2000);

            entity.Property(_ => _.Code)
                .IsRequired()
                .HasMaxLength(50);

            var seedDate = DateTime.UtcNow;
            entity.HasData(
                new TestLevel
                {
                    Id = 1,
                    Name = "Unit",
                    DisplayName = "Unit Test",
                    Description = "Tests that validate the functionality of a specific section of code, usually at the function level.",
                    Code = "UNIT",
                    SortOrder = 1,
                    CreatedAt = seedDate,
                    UpdatedAt = seedDate
                },
                new TestLevel
                {
                    Id = 2,
                    Name = "Integration",
                    DisplayName = "Integration Test",
                    Description = "Tests that validate the interaction between different components or systems.",
                    Code = "INTEGRATION",
                    SortOrder = 2,
                    CreatedAt = seedDate,
                    UpdatedAt = seedDate
                },
                new TestLevel
                {
                    Id = 3,
                    Name = "System",
                    DisplayName = "System Test",
                    Description = "Tests that validate the complete and integrated system.",
                    Code = "SYSTEM",
                    SortOrder = 3,
                    CreatedAt = seedDate,
                    UpdatedAt = seedDate
                },
                new TestLevel
                {
                    Id = 4,
                    Name = "Acceptance",
                    DisplayName = "Acceptance Test",
                    Description = "Tests that validate the system against the business requirements and user needs.",
                    Code = "ACCEPTANCE",
                    SortOrder = 4,
                    CreatedAt = seedDate,
                    UpdatedAt = seedDate
                }
            );
        }

        private void ConfigureTestCaseVersion(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<TestCaseVersion>();

            entity.HasKey(_ => _.Id);

            entity.Property(_ => _.VersionNumber)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(_ => _.Description)
                .HasMaxLength(2000);

            entity.HasOne(_ => _.TestLevel)
                .WithMany(tl => tl.TestCaseVersions)
                .HasForeignKey(_ => _.TestLevelId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(_ => new { _.Name, _.Description, _.TestLevelId, _.TestCaseId, _.VersionNumber })
                .IsUnique();

            entity.HasOne(_ => _.TestCase)
                .WithMany(tc => tc.Versions)
                .HasForeignKey(_ => _.TestCaseId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private void ConfigureTestCase(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<TestCase>();
            entity.HasKey(_ => _.Id);

            entity.Property(_ => _.Code)
                .IsRequired()
                .HasMaxLength(200);

            // Make Code unique
            entity.HasIndex(_ => _.Code)
                .IsUnique();
        }

        private void ConfigureTestResult(ModelBuilder builder)
        {
            var entity = builder.Entity<TestResult>();
            entity.HasKey(_ => _.Id);

            entity.HasOne(_ => _.TestExecutionItem)
                .WithMany(te => te.TestResults)
                .HasForeignKey(_ => _.TestExecutionItemId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(_ => _.TestCaseVersion)
                .WithMany(tc => tc.Results)
                .HasForeignKey(_ => _.TestCaseVersionId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private void ConfigureTestStatus(ModelBuilder builder)
        {
            var entity = builder.Entity<TestStatus>();
            entity.HasKey(_ => _.Id);
            entity.Property(_ => _.Code)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(_ => _.DisplayName)
                .IsRequired()
                .HasMaxLength(100);

            var seedDate = DateTime.UtcNow;
            entity.HasData(
                new TestStatus
                {
                    Id = 1,
                    Code = "PASSED",
                    DisplayName = "Passed",
                    IsSuccess = true,
                    IsTerminal = true,
                    SortOrder = 1,
                    CreatedAt = seedDate,
                    UpdatedAt = seedDate
                },
                new TestStatus
                {
                    Id = 2,
                    Code = "FAILED",
                    DisplayName = "Failed",
                    IsSuccess = false,
                    IsTerminal = true,
                    SortOrder = 2,
                    CreatedAt = seedDate,
                    UpdatedAt = seedDate
                },
                new TestStatus
                {
                    Id = 3,
                    Code = "SKIPPED",
                    DisplayName = "Skipped",
                    IsSuccess = false,
                    IsTerminal = true,
                    SortOrder = 3,
                    CreatedAt = seedDate,
                    UpdatedAt = seedDate
                },
                new TestStatus
                {
                    Id = 4,
                    Code = "IN_PROGRESS",
                    DisplayName = "In Progress",
                    IsSuccess = false,
                    IsTerminal = false,
                    SortOrder = 4,
                    CreatedAt = seedDate,
                    UpdatedAt = seedDate
                },
                new TestStatus
                {
                    Id = 5,
                    Code = "NOT EXECUTED",
                    DisplayName = "In Progress",
                    IsSuccess = false,
                    IsTerminal = false,
                    SortOrder = 4,
                    CreatedAt = seedDate,
                    UpdatedAt = seedDate
                }
            );
        }

        private void ConfigureEnvironment(ModelBuilder builder)
        {
            var entity = builder.Entity<Environment>();

            entity.HasKey(_ => _.Id);

            entity.Property(_ => _.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(_ => _.Os)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(_ => _.RunTime)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
