using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using TestManagement.API.Data;
using TestManagement.API.Features.TestExecutions;
using TestManagement.API.Models;
using TestManagement.API.Services;
using Environment = TestManagement.API.Models.Environment;
using TestManagement.API.Features.TestExecutions.Create;
using TestManagement.API.Features.TestExecutions.Update;

namespace TestManagement.API.UTest.Service
{
    public partial class TestExecutionServiceTests
    {
        // UpdateAsync の正常ケースを InlineData で複数パターン検証
        [Theory]
        [InlineData("env-update-1", "rev-u1", "TC-U1", 1, "PASS", true)]
        [InlineData("env-update-2", "rev-u2", "TC-U2", 1, "SKIP", false)]
        public async Task UpdateAsync_AddsExecutionItem_WhenExecutionExists(
            string environmentName,
            string revision,
            string testCaseCode,
            int testCaseVersion,
            string testStatusCode,
            bool _)
        {
            // Arrange
            await using var db = CreateInMemoryDbContext();

            // seed environment
            var env = new Environment { Name = environmentName };
            db.Environments.Add(env);

            // seed test case + version
            var tcVersion = new TestCaseVersion { VersionNumber = testCaseVersion };
            var tc = new TestCase
            {
                Code = testCaseCode,
            };
            tc.AddVersion("Initial Version", "Description", 1);
            db.TestCases.Add(tc);

            // seed status
            var status = new TestStatus
            {
                Code = testStatusCode,
                DisplayName = testStatusCode,
                IsSuccess = false,
                IsFailed = false,
                IsSkipped = true,
                IsExcluded = false,
                IsInProgress = false
            };
            db.TestStatuses.Add(status);

            await db.SaveChangesAsync();

            var service = new TestExecutionService(db, NullLogger<TestExecutionService>.Instance);

            var initialRequest = new CreateTestExecutionRequest
            {
                Environment = environmentName,
                Revision = revision,
                ExecutedAt = DateTime.UtcNow.AddDays(-1),
                TestCases = new List<TestCaseExecution>
                {
                    new TestCaseExecution
                    {
                        TestCaseCode = testCaseCode,
                        TestCaseVersion = testCaseVersion,
                        TestStatusCode = testStatusCode
                    }
                }
            };

            // create initial execution
            var created = await service.CreateAsync(initialRequest, CancellationToken.None);

            var updateRequest = new UpdateTestExecutionRequest
            {
                Environment = environmentName,
                Revision = revision,
                ExecutedAt = DateTime.UtcNow,
                TestCases = new List<TestCaseExecution>
                {
                    new TestCaseExecution
                    {
                        TestCaseCode = testCaseCode,
                        TestCaseVersion = testCaseVersion,
                        TestStatusCode = testStatusCode
                    }
                }
            };

            // Act
            var updateResponse = await service.UpdateAsync(updateRequest, CancellationToken.None);

            // Assert
            Assert.NotNull(updateResponse);
            Assert.Equal(revision, updateResponse.Revision);
            Assert.Equal(environmentName, updateResponse.Environment);

            var persisted = db.TestExecutions
                .Include(te => te.Items)
                    .ThenInclude(i => i.TestResults)
                .Include(te => te.Environment)
                .FirstOrDefault(te => te.Id == updateResponse.TestExecutionId);

            Assert.NotNull(persisted);
            // 更新後、Items が 2 つになるはず（作成時のアイテム + 更新で追加されたアイテム）
            Assert.True(persisted!.Items.Count >= 2);
        }
    }
}
