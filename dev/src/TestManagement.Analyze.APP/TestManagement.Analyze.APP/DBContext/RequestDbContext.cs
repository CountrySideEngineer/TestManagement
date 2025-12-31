using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TestManagement.Analyze.APP.Entities;

namespace TestManagement.Analyze.APP.DBContext;

public partial class RequestDbContext : DbContext
{
    public RequestDbContext() { }

    public RequestDbContext(DbContextOptions<RequestDbContext> options)
        : base(options)
    { }

    public virtual DbSet<Request> Requests { get; set; }

    public virtual DbSet<ResultMaster> ResultMasters { get; set; }

    public virtual DbSet<StatusMaster> StatusMasters { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Request>(entity =>
        {
            entity.HasOne(d => d.Result).WithMany(p => p.Requests).OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(d => d.Status).WithMany(p => p.Requests).OnDelete(DeleteBehavior.Restrict);
        });

        var seedDate = new DateTime(2025, 11, 21, 0, 0, 0, DateTimeKind.Utc);
        modelBuilder.Entity<StatusMaster>(_ =>
        {
            _.HasData(
                new StatusMaster { Id = 1, Name = "Not started", CreatedAt = seedDate, UpdatedAt = seedDate },
                new StatusMaster { Id = 2, Name = "Running", CreatedAt = seedDate, UpdatedAt = seedDate },
                new StatusMaster { Id = 3, Name = "Completed(success)", CreatedAt = seedDate, UpdatedAt = seedDate },
                new StatusMaster { Id = 4, Name = "Completed(failed)", CreatedAt = seedDate, UpdatedAt = seedDate }
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

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
