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
        [Fact]
        public async Task CreateAsync_CreatesANewTestCaseAndInitialVersion()
        {
            await using var context = CreateContext();
            var service = CreateService(context);

            var request = new CreateTestCaseRequest
            {
                Code = "TC-003",
                Name = "New test",
                Description = "Initial version",
                TestLevelId = (int)(await context.TestLevels.SingleAsync(_ => _.Code == "UNIT")).Id
            };

            var response = await service.CreateAsync(request, CancellationToken.None);

            Assert.Equal("TC-003", response.Code);
            Assert.Equal(1, response.VersionNumber);
            Assert.True(response.Id > 0);

            var created = await context.TestCases
                .Include(_ => _.Versions)
                .SingleAsync(_ => _.Code == "TC-003");

            Assert.Single(created.Versions);
            Assert.Equal("New test", created.Versions.Single().Name);
        }
    }
}
