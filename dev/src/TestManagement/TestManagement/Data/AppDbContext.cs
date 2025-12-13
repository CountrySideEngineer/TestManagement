using Microsoft.EntityFrameworkCore;
using TestManagement.Models;

namespace TestManagement.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<TestCase> TestCases { get; set; }
        public DbSet<TestRun> TestRuns { get; set; }
        public DbSet<XmlImportHistory> XmlImportHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relation TestCase (parent) to TestRun(child), 1 to N.
            modelBuilder.Entity<TestCase>()
                .HasMany(testCase => testCase.TestRuns)
                .WithOne(testRun => testRun.TestCase)
                .HasForeignKey(testRun => testRun.TestCaseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
