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
        public async Task GetAllAsync_ReturnsAllTestCasesWithTheirVersions()
        {
            await using var context = CreateContext();
            await SeedDataAsync(context);
            var service = CreateService(context);

            var result = await service.GetAllAsync(CancellationToken.None);

            Assert.Equal(2, result.Count);

            var first = result.Single(_ => _.Code == "TC-001");
            Assert.Equal(2, first.Versions.Count);
            Assert.Contains(first.Versions, version => version.Name == "Initial");
            Assert.Contains(first.Versions, version => version.Name == "Updated");

            var second = result.Single(_ => _.Code == "TC-002");
            Assert.Single(second.Versions);
            Assert.Contains(second.Versions, version => version.Name == "Only one");
        }
    }
}
