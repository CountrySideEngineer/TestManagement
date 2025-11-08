using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TestManagement.Model;

namespace TestManagement.Data
{
    public class TestManagementDbContext : DbContext
    {
        public TestManagementDbContext(DbContextOptions<TestManagementDbContext> options)
            : base(options)
        {
        }

        public DbSet<Project> Projects => Set<Project>();
        public DbSet<TestSuite> TestSuites => Set<TestSuite>();
        public DbSet<TestLevel> TestLevels => Set<TestLevel>();
        public DbSet<TestCase> TestCases => Set<TestCase>();
        public DbSet<Tester> Testers => Set<Tester>();
        public DbSet<TestRun> TestRuns => Set<TestRun>();
        public DbSet<TestResult> TestResults => Set<TestResult>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Project>()
                        .HasMany(p => p.TestSuites)
                        .WithOne(s => s.Project)
                        .HasForeignKey(s => s.ProjectId)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Project>()
                .HasMany(p => p.TestRuns)
                .WithOne(r => r.Project)
                .HasForeignKey(r => r.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TestSuite>()
                .HasMany(s => s.TestCases)
                .WithOne(c => c.TestSuite)
                .HasForeignKey(c => c.TestSuiteId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TestLevel>()
                .HasMany(l => l.TestCases)
                .WithOne(c => c.TestLevel)
                .HasForeignKey(c => c.TestLevelId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TestCase>()
                .HasMany(c => c.TestResults)
                .WithOne(rr => rr.TestCase)
                .HasForeignKey(rr => rr.TestCaseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TestRun>()
                .HasMany(r => r.TestResults)
                .WithOne(rr => rr.TestRun)
                .HasForeignKey(rr => rr.TestRunId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Tester>()
                .HasMany(t => t.TestRuns)
                .WithOne(r => r.Tester)
                .HasForeignKey(r => r.TesterId)
                .OnDelete(DeleteBehavior.SetNull);

            var statusConverter = new EnumToStringConverter<TestStatus>();
            modelBuilder.Entity<TestResult>()
                .Property(rr => rr.Status)
                .HasConversion(statusConverter)
                .HasMaxLength(32)
                .HasColumnName("Status");

            modelBuilder.Entity<TestRun>().Property(r => r.ExecutedAt).HasColumnType("timestamptz");
            modelBuilder.Entity<Project>().Property(p => p.CreatedAt).HasColumnType("timestamptz");
            modelBuilder.Entity<Project>().Property(p => p.UpdatedAt).HasColumnType("timestamptz");
            modelBuilder.Entity<TestCase>().Property(c => c.CreatedAt).HasColumnType("timestamptz");
            modelBuilder.Entity<TestCase>().Property(c => c.UpdatedAt).HasColumnType("timestamptz");
            modelBuilder.Entity<TestSuite>().Property(s => s.CreatedAt).HasColumnType("timestamptz");

            // Optional: Seed TestLevel (Unit/Integration/System)
            modelBuilder.Entity<TestLevel>().HasData(
                new TestLevel { Id = 1, Name = "Unit", Description = "Unit tests" },
                new TestLevel { Id = 2, Name = "Integration", Description = "Integration tests" },
                new TestLevel { Id = 3, Name = "System", Description = "System tests" }
            );
        }
    }
}
