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
        }
    }
}
