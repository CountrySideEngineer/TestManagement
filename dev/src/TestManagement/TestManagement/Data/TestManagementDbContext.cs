using Microsoft.EntityFrameworkCore;
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
        }
    }
}
