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
        [Theory]
        [InlineData("Updated")]
        [InlineData("Only one")]
        public async Task GetByVersionIdAsync_ReturnsTheSpecifiedVersionWithNavigationData(string name)
        {
            await using var context = CreateContext();
            await SeedDataAsync(context);
            var service = CreateService(context);

            var version = await context.TestCaseVersions.SingleAsync(_ => _.Name == name);
            var result = await service.GetByVersionIdAsync(version.Id, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(version.Id, result!.Id);
            Assert.NotNull(result.TestLevel);
            Assert.NotNull(result.TestCase);
        }
    }
}
