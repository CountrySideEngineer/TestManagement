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
        public async Task CreateVersionForExistingCaseAsync_AddsANewVersionToExistingCase()
        {
            await using var context = CreateContext();
            var unitLevel = await context.TestLevels.SingleAsync(_ => _.Code == "UNIT");

            var existingCase = new TestCase
            {
                Code = "TC-004",
                IsActive = true
            };
            existingCase.AddVersion("Initial", "Initial description", unitLevel.Id);
            context.TestCases.Add(existingCase);
            await context.SaveChangesAsync();

            var service = CreateService(context);
            var request = new CreateTestCaseRequest
            {
                Code = "TC-004",
                Name = "Second version",
                Description = "New description",
                TestLevelId = (int)unitLevel.Id
            };

            var response = await service.CreateVersionForExistingCaseAsync(request, CancellationToken.None);

            Assert.Equal("TC-004", response.Code);
            Assert.Equal(2, response.VersionNumber);

            var reloaded = await context.TestCases
                .Include(_ => _.Versions)
                .SingleAsync(_ => _.Code == "TC-004");

            Assert.Equal(2, reloaded.Versions.Count);
            Assert.Contains(reloaded.Versions, version => version.Name == "Second version" && version.IsLatest);
        }

        [Fact]
        public async Task CreateVersionForExistingCaseAsync_ThrowsWhenTheTargetTestCaseDoesNotExist()
        {
            await using var context = CreateContext();
            var service = CreateService(context);
            var unitLevel = await context.TestLevels.SingleAsync(_ => _.Code == "UNIT");

            var request = new CreateTestCaseRequest
            {
                Code = "TC-999",
                Name = "Second version",
                Description = "New description",
                TestLevelId = (int)unitLevel.Id
            };

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => service.CreateVersionForExistingCaseAsync(request, CancellationToken.None));

            Assert.Equal("TestCase TC-999 not found.", exception.Message);
            Assert.False(await context.TestCases.AnyAsync());
        }
    }
}
