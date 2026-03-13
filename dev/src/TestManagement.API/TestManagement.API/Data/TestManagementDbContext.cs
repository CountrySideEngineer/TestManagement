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

        public DbSet<Environment> Environments { get;set; }
        public DbSet<TestCase> TestCases { get; set; }
        public DbSet<TestCaseVersion> TestCaseVersions { get; set; }
        public DbSet<TestExecution> TestExecutions { get; set; }
        public DbSet<TestLevel> TestLevels { get; set; }
        public DbSet<TestResult> TestResults { get; set; }
        public DbSet<TestStatus> TestStatuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureEnvironment(modelBuilder);
            ConfigureTestCase(modelBuilder);
            ConfigureTestCaseVersion(modelBuilder);
            ConfigureTestExecution(modelBuilder);
            ConfigureTestLevel(modelBuilder);
            ConfigureTestResult(modelBuilder);
            ConfigureTestStatus(modelBuilder);
        }

        private void ConfigureEnvironment(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<Environment>();
            // Id as primary key.
            entity.HasKey(_ => _.Id);

            // Name is required and has max length of 100.
            entity.Property(_ => _.Name).IsRequired()
                .HasMaxLength(100);

            // OS has max length of 100.
            entity.Property(_ => _.Os)
                .HasMaxLength(100);

            entity.Property(_ => _.RunTime)
                .HasMaxLength(100);

            entity.Property(_ => _.CreatedAt)
                .HasColumnType("timestamp with time zone")
                .HasDefaultValue("NOW()");

            // Environments with the same name, OS, and runtime
            // are considered identical, so they must not be registered as duplicates.
            entity.HasIndex(_ => new { _.Name, _.Os, _.RunTime }).IsUnique();
        }

        private void ConfigureTestLevel(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<TestLevel>();

            entity.HasKey(_ => _.Id);

            entity.Property(_ => _.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(_ => _.Description)
                .HasMaxLength(1000);

            entity.HasIndex(_ => _.Name)
                .IsUnique();


            // Add seed TestLevel class (Unit, Integration, System, Acceptance)
            var seedDate = new DateTime(2025, 11, 21, 0, 0, 0, DateTimeKind.Utc);
            entity.HasData(
                new TestLevel { Id = 1, Name = "Unit", Description = "Unit Level Testing", CreatedAt = seedDate, UpdatedAt = seedDate },
                new TestLevel { Id = 2, Name = "Integration", Description = "Integration Level Testing", CreatedAt = seedDate, UpdatedAt = seedDate },
                new TestLevel { Id = 3, Name = "System", Description = "System Level Testing", CreatedAt = seedDate, UpdatedAt = seedDate },
                new TestLevel { Id = 4, Name = "Acceptance", Description = "Acceptance Level Testing", CreatedAt = seedDate, UpdatedAt = seedDate }
            );
        }

        private void ConfigureTestResult(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<TestResult>();

            entity.HasKey(_ => _.Id);

            entity.HasIndex( _ => new { _.TestExecutionItemId, _.TestCaseVersionId } )
                .IsUnique();

            entity.HasOne(_ => _.TestExecution)
                .WithMany(_ => _.TestResults)
                .HasForeignKey(_ => _.TestExecutionItemId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(_ => _.TestCaseVersion)
                .WithMany(_ => _.Results)
                .HasForeignKey(_ => _.TestCaseVersionId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(_ => _.Status)
                .WithMany(_ => _.TestResults)
                .HasForeignKey(_ => _.StatusId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private void ConfigureTestCase(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<TestCase>();

            modelBuilder.Entity<TestCase>()
                .HasMany(_ => _.Versions)
                .WithOne(_ => _.TestCase)
                .HasForeignKey(_ => _.TestCaseId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private void ConfigureTestCaseVersion(ModelBuilder builder)
        {
            var entity = builder.Entity<TestCaseVersion>();

            entity.HasKey(_ => _.Id);

            entity.Property(_ => _.Name)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(_ => _.VersionNumber)
                .IsRequired();

            entity.Property(_ => _.Description)
                .HasMaxLength(2000);

            entity.Property(_ => _.IsLatest)
                .IsRequired();

            entity.HasOne<TestCase>()
                .WithMany(_ => _.Versions)
                .HasForeignKey(_ => _.TestCaseId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne<TestLevel>()
                .WithMany(_ => _.TestCaseVersions)
                .HasForeignKey(_ => _.TestLevelId)
                .OnDelete(DeleteBehavior.Restrict);

            //Unique constraints
            entity.HasIndex(_ => new { _.TestCaseId, _.VersionNumber })
                .IsUnique();
        }

        private void ConfigureTestExecution(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<TestExecution>();
            entity.HasKey(_ => _.Id);

            entity.Property(_ => _.Revision)
                .IsRequired()
                .HasMaxLength(255);

            entity.HasIndex(_ => new { _.Revision, _.EnvironmentId, _.ExecutedAt } )
                .IsUnique();

            entity.HasOne(exe => exe.Environment)
                .WithMany(env => env.TestExecutions)
                .HasForeignKey(exe => exe.EnvironmentId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private void ConfigureTestStatus(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<TestStatus>();

            entity.HasKey(_ => _.Id);

            entity.Property(_ => _.Code)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(_ => _.DisplayName)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(_ => _.IsSuccess)
                .IsRequired()
                .HasDefaultValue(false);

            entity.Property(_ => _.IsTerminal)
                .IsRequired()
                .HasDefaultValue(false);

        }
    }
}
