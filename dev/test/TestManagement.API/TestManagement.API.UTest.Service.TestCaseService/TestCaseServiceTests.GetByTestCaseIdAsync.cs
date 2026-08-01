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
        [InlineData("TC-001", 2)]
        [InlineData("TC-002", 1)]
        public async Task GetByTestCaseIdAsync_ReturnsTheTestCaseAndAllVersions(
            string testCaseCode, 
            int expectedVersionCount
            )
        {
            await using var context = CreateContext();
            await SeedDataAsync(context);
            var service = CreateService(context);

            var testCase = await context.TestCases.SingleAsync(_ => _.Code == testCaseCode);
            var response = await service.GetByTestCaseIdAsync(testCase.Id, CancellationToken.None);

            Assert.Equal(testCase.Code, response.Code);
            Assert.Equal(expectedVersionCount, response.Versions.Count);
        }
    }
}
