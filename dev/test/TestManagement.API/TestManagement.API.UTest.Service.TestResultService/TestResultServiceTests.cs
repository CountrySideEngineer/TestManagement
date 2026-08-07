using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;
using TestManagement.API.Data;
using TestManagement.API.Features.TestResult.Create;
using TestManagement.API.Features.TestResult.Get;
using TestManagement.API.Models;
using TestManagement.API.Services;
using Xunit;

namespace TestManagement.API.Tests.Service
{
    public partial class TestResultServiceTests
    {
        private static TestManagementDbContext CreateContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<TestManagementDbContext>()
                .UseInMemoryDatabase(dbName)
                .ConfigureWarnings(_ => _.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            var ctx = new TestManagementDbContext(options);
            // Ensure model seed data from OnModelCreating is applied.
            ctx.Database.EnsureCreated();
            return ctx;
        }
    }
}