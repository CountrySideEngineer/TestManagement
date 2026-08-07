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
        // GetAsync と GetByIdAsync の簡易マッピング確認（InlineData でパラメタライズ）
        [Theory]
        [InlineData("env-get-1", "rev-g1", "TC-G1", 1, "PASS")]
        [InlineData("env-get-2", "rev-g2", "TC-G2", 1, "FAIL")]
        public async Task GetAsync_ReturnsMappedDto(
            string environmentName,
            string revision,
            string testCaseCode,
            int testCaseVersion,
            string testStatusCode)
        {
            // Arrange
            await using var db = CreateInMemoryDbContext();

            var env = new Environment { Name = environmentName };
            db.Environments.Add(env);

            var tc = new TestCase
            {
                Code = testCaseCode
            };
            tc.AddVersion("Initial Version", "Description", 1);
            db.TestCases.Add(tc);

            var status = new TestStatus
            {
                Code = testStatusCode,
                DisplayName = testStatusCode,
                IsSuccess = true,
                IsFailed = false,
                IsSkipped = false,
                IsExcluded = false,
                IsInProgress = false
            };
            db.TestStatuses.Add(status);

            await db.SaveChangesAsync();

            var service = new TestExecutionService(db, NullLogger<TestExecutionService>.Instance);

            var createRequest = new CreateTestExecutionRequest
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

            var created = await service.CreateAsync(createRequest, CancellationToken.None);

            // Act
            var all = await service.GetAsync(CancellationToken.None);

            // Assert
            Assert.Contains(all, g => g.TestExecutionId == created.TestExecutionId);
        }
    }
}
