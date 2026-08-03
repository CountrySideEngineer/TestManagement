using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using TestManagement.API.Data;
using TestManagement.API.Features.TestExecutions.Create;
using TestManagement.API.Features.TestExecutions.Update;
using TestManagement.API.Models;
using TestManagement.API.Services;
using Environment = TestManagement.API.Models.Environment;

namespace TestManagement.API.UTest.Service
{
    public class TestExecutionServiceTests
    {
        private static TestManagementDbContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<TestManagementDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new TestManagementDbContext(options);
        }

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
                TestCases = new List<TestManagement.API.Features.TestExecutions.TestCaseExecution>
                {
                    new TestManagement.API.Features.TestExecutions.TestCaseExecution
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
                TestCases = new List<TestManagement.API.Features.TestExecutions.TestCaseExecution>
                {
                    new TestManagement.API.Features.TestExecutions.TestCaseExecution
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
                TestCases = new List<TestManagement.API.Features.TestExecutions.TestCaseExecution>
                {
                    new TestManagement.API.Features.TestExecutions.TestCaseExecution
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

        // GetAsync と GetByIdAsync の簡易マッピング確認（InlineData でパラメタライズ）
        [Theory]
        [InlineData("env-get-1", "rev-g1", "TC-G1", 1, "PASS")]
        [InlineData("env-get-2", "rev-g2", "TC-G2", 1, "FAIL")]
        public async Task GetAsync_And_GetByIdAsync_ReturnsMappedDto(
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
                TestCases = new List<TestManagement.API.Features.TestExecutions.TestCaseExecution>
                {
                    new TestManagement.API.Features.TestExecutions.TestCaseExecution
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
            var single = await service.GetByIdAsync(created.TestExecutionId, CancellationToken.None);

            // Assert
            Assert.Contains(all, g => g.TestExecutionId == created.TestExecutionId);
            Assert.Equal(created.TestExecutionId, single.TestExecutionId);
            Assert.Equal(environmentName, single.Environment);
            Assert.NotEmpty(single.TestCases);
            Assert.Contains(single.TestCases, tcItem => tcItem.TestCaseCode == testCaseCode);
        }
    }
}
