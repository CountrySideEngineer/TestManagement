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
    public class TestCaseServiceTests
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
            Assert.Contains(first.Versions, version => version.Name == "Updated");
        }

        [Fact]
        public async Task GetAllLatestVersionAsync_ReturnsOnlyTheLatestVersionPerTestCase()
        {
            await using var context = CreateContext();
            await SeedDataAsync(context);
            var service = CreateService(context);

            var result = await service.GetAllLatestVersionAsync(CancellationToken.None);

            Assert.Equal(2, result.Count);
            Assert.All(result, response => Assert.Single(response.Versions));
            Assert.Contains(result, response => response.Code == "TC-001" && response.Versions.Single().VersionNumber == 2);
        }

        [Fact]
        public async Task GetByTestLevelIdAsync_ReturnsOnlyVersionsForTheRequestedLevel()
        {
            await using var context = CreateContext();
            await SeedDataAsync(context);
            var service = CreateService(context);

            var unitLevel = await context.TestLevels.SingleAsync(_ => _.Code == "UNIT");
            var result = await service.GetByTestLevelIdAsync((int)unitLevel.Id, CancellationToken.None);

            Assert.Single(result);
            Assert.Equal(unitLevel.Id, result.Single().TestLevelId);
            Assert.Equal("Initial", result.Single().Name);
        }

        [Fact]
        public async Task GetByTestCaseIdAsync_ReturnsTheTestCaseAndAllVersions()
        {
            await using var context = CreateContext();
            await SeedDataAsync(context);
            var service = CreateService(context);

            var testCase = await context.TestCases.SingleAsync(_ => _.Code == "TC-001");
            var response = await service.GetByTestCaseIdAsync(testCase.Id, CancellationToken.None);

            Assert.Equal(testCase.Code, response.Code);
            Assert.Equal(2, response.Versions.Count);
            Assert.Contains(response.Versions, version => version.VersionNumber == 2);
        }

        [Fact]
        public async Task GetByVersionIdAsync_ReturnsTheSpecifiedVersionWithNavigationData()
        {
            await using var context = CreateContext();
            await SeedDataAsync(context);
            var service = CreateService(context);

            var version = await context.TestCaseVersions.SingleAsync(_ => _.Name == "Updated");
            var result = await service.GetByVersionIdAsync(version.Id, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(version.Id, result!.Id);
            Assert.NotNull(result.TestLevel);
            Assert.NotNull(result.TestCase);
        }

        [Fact]
        public async Task GetLatestVersionByTestCaseIdAsync_ReturnsTheLatestVersion()
        {
            await using var context = CreateContext();
            await SeedDataAsync(context);
            var service = CreateService(context);

            var testCase = await context.TestCases.SingleAsync(_ => _.Code == "TC-001");
            var result = await service.GetLatestVersionByTestCaseIdAsync(testCase.Id, CancellationToken.None);

            Assert.Equal(2, result.VersionNumber);
            Assert.Equal("Updated", result.Name);
        }

        [Fact]
        public async Task GetLatestVersionByTestCaseIdAsync_ThrowsWhenNoVersionsExist()
        {
            await using var context = CreateContext();
            var service = CreateService(context);

            await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetLatestVersionByTestCaseIdAsync(999, CancellationToken.None));
        }

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