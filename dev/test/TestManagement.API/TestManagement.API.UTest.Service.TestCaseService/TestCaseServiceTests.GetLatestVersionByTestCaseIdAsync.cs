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
        [InlineData("TC-001", 2, "Updated")]
        [InlineData("TC-002", 1, "Only one")]
        public async Task GetLatestVersionByTestCaseIdAsync_ReturnsTheLatestVersion(string code, int versionNumber, string name)
        {
            await using var context = CreateContext();
            await SeedDataAsync(context);
            var service = CreateService(context);

            var testCase = await context.TestCases.SingleAsync(_ => _.Code == code);
            var result = await service.GetLatestVersionByTestCaseIdAsync(testCase.Id, CancellationToken.None);

            Assert.Equal(versionNumber, result.VersionNumber);
            Assert.Equal(name, result.Name);
        }

        [Fact]
        public async Task GetLatestVersionByTestCaseIdAsync_ThrowsWhenNoVersionsExist()
        {
            await using var context = CreateContext();
            var service = CreateService(context);

            await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetLatestVersionByTestCaseIdAsync(999, CancellationToken.None));
        }
    }
}
