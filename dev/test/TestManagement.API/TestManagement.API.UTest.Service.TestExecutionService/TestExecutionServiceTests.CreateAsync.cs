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
        // CreateAsync の正常ケースを InlineData で複数パターン検証
        [Theory]
        [InlineData("env-a", "rev-1", "TC-001", 1, "PASS", true)]
        [InlineData("env-b", "rev-2", "TC-002", 2, "FAIL", false)]
        public async Task CreateAsync_CreatesExecution_WhenDataIsValid(
            string environmentName,
            string revision,
            string testCaseCode,
            int testCaseVersion,
            string testStatusCode,
            bool expectedIsSuccess)
        {
            // Arrange
            await using var db = CreateInMemoryDbContext();

            // seed environment
            var env = new Environment { Name = environmentName };
            db.Environments.Add(env);

            // seed test case + version
            var tc = new TestCase
            {
                Code = testCaseCode
            };
            for (int index = 0; index < testCaseVersion; index++)
            {
                string versionName = $"Version {index + 1}";
                string versionDesc = $"Description for version {index + 1}";
                tc.AddVersion(versionName, versionDesc, 1);
            }
            db.TestCases.Add(tc);

            // seed status
            var status = new TestStatus
            {
                Code = testStatusCode,
                DisplayName = testStatusCode,
                IsSuccess = expectedIsSuccess,
                IsFailed = !expectedIsSuccess,
                IsSkipped = false,
                IsExcluded = false,
                IsInProgress = false
            };
            db.TestStatuses.Add(status);

            await db.SaveChangesAsync();

            var service = new TestExecutionService(db, NullLogger<TestExecutionService>.Instance);

            var executedAt = DateTime.UtcNow;

            var request = new CreateTestExecutionRequest
            {
                Environment = environmentName,
                Revision = revision,
                ExecutedAt = executedAt,
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
            var response = await service.CreateAsync(request, CancellationToken.None);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(environmentName, response.Environment);
            Assert.Equal(executedAt, response.ExecutedAt);
            Assert.NotEqual(0, response.TestExecutionId);

            var persisted = db.TestExecutions
                .Include(te => te.Items)
                    .ThenInclude(i => i.TestResults)
                .Include(te => te.Environment)
                .FirstOrDefault(te => te.Revision == revision && te.Environment!.Name == environmentName);

            Assert.NotNull(persisted);
            Assert.Equal(revision, persisted!.Revision);
            Assert.Equal(environmentName, persisted.Environment!.Name);
            Assert.NotEmpty(persisted.Items);
            Assert.NotEmpty(persisted.Items.SelectMany(i => i.TestResults));
        }
    }
}
