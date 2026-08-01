using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;
using TestManagement.API.Data;
using TestManagement.API.Features.TestCases.Create;
using TestManagement.API.Features.TestCases.Get;
using TestManagement.API.Features.TestCases.Update;
using TestManagement.API.Models;
using TestManagement.API.Services;

namespace TestManagement.API.Tests.Service
{
    public partial class TestCaseServiceTests
    {

        private static TestManagementDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<TestManagementDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .ConfigureWarnings(_ => _.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            var context = new TestManagementDbContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        private static TestCaseService CreateService(TestManagementDbContext context)
            => new(context, NullLogger<TestCaseService>.Instance);

        private static async Task SeedDataAsync(TestManagementDbContext context)
        {
            var unitLevel = await context.TestLevels.SingleAsync(_ => _.Code == "UNIT");
            var integrationLevel = await context.TestLevels.SingleAsync(_ => _.Code == "INTEGRATION");

            var firstCase = new TestCase
            {
                Code = "TC-001",
                IsActive = true
            };
            firstCase.AddVersion("Initial", "First version", unitLevel.Id);
            firstCase.AddVersion("Updated", "Second version", integrationLevel.Id);
            context.TestCases.Add(firstCase);

            var secondCase = new TestCase
            {
                Code = "TC-002",
                IsActive = true
            };
            secondCase.AddVersion("Only one", "Second case version", integrationLevel.Id);
            context.TestCases.Add(secondCase);

            await context.SaveChangesAsync();
        }
    }
}