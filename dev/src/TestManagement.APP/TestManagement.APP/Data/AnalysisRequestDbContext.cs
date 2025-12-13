using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using TestManagement.APP.Models.TestAnalysis;

namespace TestManagement.APP.Data
{
    public class AnalysisRequestDbContext : DbContext
    {
        public AnalysisRequestDbContext(DbContextOptions<AnalysisRequestDbContext> options) : base(options)
        {
        }

        public DbSet<Models.TestAnalysis.Request> Requests { get; set; } = null!;
        public DbSet<Models.TestAnalysis.StatusMaster> StatusMasters { get; set; } = null!;
        public DbSet<Models.TestAnalysis.ResultMaster> ResultMasters { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships and constraints if needed
            modelBuilder.Entity<Models.TestAnalysis.ResultMaster>()
                .HasMany(_ => _.Requests)
                .WithOne(_ => _.Result)
                .HasForeignKey(_ => _.ResultId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Models.TestAnalysis.StatusMaster>()
                .HasMany(_ => _.Requests)
                .WithOne(_ => _.Status)
                .HasForeignKey(_ => _.StatusId)
                .OnDelete(DeleteBehavior.Restrict);

            var seedDate = new DateTime(2025, 11, 21, 0, 0, 0, DateTimeKind.Utc);
            modelBuilder.Entity<StatusMaster>(_ =>
            {
                _.HasData(
                    new StatusMaster { Id = 1, Name = "Not started", CreatedAt = seedDate, UpdatedAt = seedDate },
                    new StatusMaster { Id = 2, Name = "Running", CreatedAt = seedDate, UpdatedAt = seedDate },
                    new StatusMaster { Id = 3, Name = "Finished", CreatedAt = seedDate, UpdatedAt = seedDate }
                    );
            });

            modelBuilder.Entity<ResultMaster>(_ =>
            {
                _.HasData(
                    new ResultMaster { Id = 1, Name = "Unknown", CreatedAt = seedDate, UpdatedAt = seedDate },
                    new ResultMaster { Id = 2, Name = "Passed", CreatedAt = seedDate, UpdatedAt = seedDate },
                    new ResultMaster { Id = 3, Name = "Failed", CreatedAt = seedDate, UpdatedAt = seedDate }
                    );
            });
        }

    }
}
