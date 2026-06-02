using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System.Data.Common;
using System.Security.Cryptography.X509Certificates;
using TestManagement.API.Models;
using Environment = TestManagement.API.Models.Environment;

namespace TestManagement.API.Data
{
    /// <summary>
    /// Entity Framework Core DbContext for the Test Management domain.
    /// Configures entity mappings and seeds initial lookup data.
    /// </summary>
    public class TestManagementDbContext : DbContext
    {
        /// <summary>
        /// Creates a new instance of <see cref="TestManagementDbContext"/> using the specified options.
        /// </summary>
        public TestManagementDbContext(DbContextOptions<TestManagementDbContext> options) : base(options) { }

        /// <summary>
        /// DbSet of test levels.
        /// </summary>
        public DbSet<TestLevel> TestLevels { get; set; }

        /// <summary>
        /// DbSet of test case versions.
        /// </summary>
        public DbSet<TestCaseVersion> TestCaseVersions { get; set; }

        /// <summary>
        /// DbSet of test cases.
        /// </summary>
        public DbSet<TestCase> TestCases { get; set; }

        /// <summary>
        /// DbSet of test results.
        /// </summary>
        public DbSet<CreateTestResultRequest> TestResults { get; set; }

        /// <summary>
        /// DbSet of test statuses.
        /// </summary>
        public DbSet<TestStatus> TestStatuses { get; set; }

        /// <summary>
        /// DbSet of environments.
        /// </summary>
        public DbSet<Environment> Environments { get; set; }

        /// <summary>
        /// DbSet of environment versions (historical OS/runtime configurations for environments).
        /// </summary>
        public DbSet<EnvironmentVersion> EnvironmentVersions { get; set; }

        /// <summary>
        /// DbSet of test execution items.
        /// </summary>
        public DbSet<TestExecutionItem> TestExecutionItems { get; set; }

        /// <summary>
        /// DbSet of test executions.
        /// </summary>
        public DbSet<TestExecution> TestExecutions { get; set; }

        /// <summary>
        /// Applies configuration for all entities when the model is being created.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureTestLevel(modelBuilder);
            ConfigureTestCaseVersion(modelBuilder);
            ConfigureTestCase(modelBuilder);
            ConfigureTestResult(modelBuilder);
            ConfigureTestStatus(modelBuilder);
            ConfigureEnvironmentVersion(modelBuilder);
            ConfigureEnvironment(modelBuilder);
            ConfigureTestExecutionItem(modelBuilder);
            ConfigureTestExecution(modelBuilder);
        }

        /// <summary>
        /// Configures the TestLevel entity mapping and seeds initial test levels.
        /// </summary>
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

            var seedDate = new DateTime(2025, 11, 21, 0, 0, 0, DateTimeKind.Utc);
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

        /// <summary>
        /// Configures the TestCaseVersion entity mapping and relationships.
        /// </summary>
        private void ConfigureTestCaseVersion(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<TestCaseVersion>();

            entity.HasKey(_ => _.Id);

            entity.Property(_ => _.VersionNumber)
                .IsRequired();

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

        /// <summary>
        /// Configures the TestCase entity mapping and constraints.
        /// </summary>
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

        /// <summary>
        /// Configures the TestResult entity mapping and relationships.
        /// </summary>
        private void ConfigureTestResult(ModelBuilder builder)
        {
            var entity = builder.Entity<CreateTestResultRequest>();
            entity.HasKey(_ => _.Id);

            entity.HasOne(_ => _.TestExecutionItem)
                .WithMany(te => te.TestResults)
                .HasForeignKey(_ => _.TestExecutionItemId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(_ => _.TestCaseVersion)
                .WithMany(tc => tc.Results)
                .HasForeignKey(_ => _.TestCaseVersionId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(_ => _.Status)
                .WithMany(_ => _.TestResults )
                .HasForeignKey(_ => _.StatusId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(_ => new { _.TestExecutionItemId, _.TestCaseVersionId, _.ExecutedAt })
                .IsUnique();
        }

        /// <summary>
        /// Configures the TestStatus entity mapping and seeds default statuses.
        /// </summary>
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

            var seedDate = new DateTime(2025, 11, 21, 0, 0, 0, DateTimeKind.Utc);
            entity.HasData(
                new TestStatus
                {
                    Id = 1,
                    Code = "PASSED",
                    DisplayName = "Passed",
                    IsSuccess = true,
                    IsFailed = false,
                    IsSkipped = false,
                    IsExcluded = false,
                    IsInProgress = false,
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
                    IsFailed = true,
                    IsSkipped = false,
                    IsExcluded = false,
                    IsInProgress = false,
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
                    IsFailed = false,
                    IsSkipped = true,
                    IsExcluded = false,
                    IsInProgress = false,
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
                    IsFailed = false,
                    IsSkipped = false,
                    IsExcluded = false,
                    IsInProgress = true,
                    IsTerminal = false,
                    SortOrder = 4,
                    CreatedAt = seedDate,
                    UpdatedAt = seedDate
                },
                new TestStatus
                {
                    Id = 5,
                    Code = "NOT EXECUTED",
                    DisplayName = "Not executed",
                    IsSuccess = false,
                    IsFailed = false,
                    IsSkipped = false,
                    IsExcluded = false,
                    IsInProgress = false,
                    IsTerminal = false,
                    SortOrder = 5,
                    CreatedAt = seedDate,
                    UpdatedAt = seedDate
                },
                new TestStatus
                {
                    Id = 6,
                    Code = "EXCLUDED",
                    DisplayName = "Excluded",
                    IsSuccess = false,
                    IsFailed = false,
                    IsSkipped = false,
                    IsExcluded = true,
                    IsInProgress = false,
                    IsTerminal = false,
                    SortOrder = 6,
                    CreatedAt = seedDate,
                    UpdatedAt = seedDate
                }
            );
        }

        /// <summary>
        /// Configures the Environment entity mapping.
        /// </summary>
        private void ConfigureEnvironment(ModelBuilder builder)
        {
            var entity = builder.Entity<Environment>();

            entity.HasKey(_ => _.Id);

            entity.Property(_ => _.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasIndex(_ => new { _.Name })
                .IsUnique();
        }

        /// <summary>
        /// Configures the EnvironmentVersion entity mapping and relationships.
        /// </summary>
        private void ConfigureEnvironmentVersion(ModelBuilder builder)
        {
            var entity = builder.Entity<EnvironmentVersion>();
            entity.HasKey(_ => _.Id);

            entity.Property(_ => _.Os)
                .HasMaxLength(200);

            entity.Property(_ => _.RunTime)
                .HasMaxLength(2000);

            entity.Property(_ => _.VersionNumber)
                .IsRequired();

            entity.HasIndex(_ => new { _.Os, _.RunTime, _.EnvironmentId, _.VersionNumber })
                .IsUnique();

            entity.HasOne(_ => _.Environment)
                .WithMany(e => e.Versions)
                .HasForeignKey(_ => _.EnvironmentId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        /// <summary>
        /// Configures the TestExecutionItem entity mapping and relationships.
        /// </summary>
        private void ConfigureTestExecutionItem(ModelBuilder builder)
        {
            var entity = builder.Entity<TestExecutionItem>();
            entity.HasKey(_ => _.Id);

            entity.HasOne(_ => _.TestExecution)
                .WithMany(te => te.Items)
                .HasForeignKey(_ => _.TestExecutionId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(_ => _.EnvironmentVersion)
                .WithMany(e => e.TestExecutionItems)
                .HasForeignKey(_ => _.EnvironmentId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        /// <summary>
        /// Configures the TestExecution entity mapping.
        /// </summary>
        private void ConfigureTestExecution(ModelBuilder builder)
        {
            var entity = builder.Entity<TestExecution>();
            entity.HasKey(_ => _.Id);

            entity.HasIndex(_ => new { _.Revision, _.EnvironmentId })
                .IsUnique();
        }
    }
}
