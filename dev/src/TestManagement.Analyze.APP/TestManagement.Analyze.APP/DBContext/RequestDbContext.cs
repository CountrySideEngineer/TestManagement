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

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
