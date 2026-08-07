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
        [InlineData("UNIT", "Initial", 1)]
        [InlineData("INTEGRATION", "Updated", 2)]
        public async Task GetByTestLevelIdAsync_ReturnsOnlyVersionsForTheRequestedLevel(string testLevel, string expectedName, int expectedNum)
        {
            await using var context = CreateContext();
            await SeedDataAsync(context);
            var service = CreateService(context);

            var unitLevel = await context.TestLevels.SingleAsync(_ => _.Code == testLevel);
            var result = await service.GetByTestLevelIdAsync((int)unitLevel.Id, CancellationToken.None);

            Assert.Equal(expectedNum, result.Count);
            Assert.Equal(unitLevel.Id, result.ElementAt(0).TestLevelId);
            Assert.Equal(expectedName, result.ElementAt(0).Name);
        }
    }
}
