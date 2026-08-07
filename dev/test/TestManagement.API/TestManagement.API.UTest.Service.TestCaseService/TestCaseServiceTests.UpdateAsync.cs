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
        public async Task UpdateAsync_AddsANewVersionAndReturnsUpdatedValues()
        {
            await using var context = CreateContext();
            var unitLevel = await context.TestLevels.SingleAsync(_ => _.Code == "UNIT");

            var existingCase = new TestCase
            {
                Code = "TC-005",
                IsActive = true
            };
            existingCase.AddVersion("Initial", "Initial description", unitLevel.Id);
            context.TestCases.Add(existingCase);
            await context.SaveChangesAsync();

            var service = CreateService(context);
            var request = new UpdateTestCaseRequest
            {
                Code = "TC-005",
                Name = "Updated name",
                Description = "Updated description"
            };

            var response = await service.UpdateAsync(request, CancellationToken.None);

            Assert.Equal("TC-005", response.Code);
            Assert.Equal("Updated name", response.Name);
            Assert.Equal("Updated description", response.Description);
            Assert.Equal(2, response.VersionNumber);

            var reloaded = await context.TestCases
                .Include(_ => _.Versions)
                .SingleAsync(_ => _.Code == "TC-005");

            Assert.Equal(2, reloaded.Versions.Count);
            Assert.Equal(2, reloaded.Versions.Max(_ => _.VersionNumber));
        }
    }
}
