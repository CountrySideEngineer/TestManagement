using Microsoft.EntityFrameworkCore;
using TestManagement.API.Data;
using TestManagement.API.Features.TestCases.Create;
using TestManagement.API.Features.testExecutions.Create;
using TestManagement.API.Features.TestExecutions.Create;
using TestManagement.API.Models;

namespace TestManagement.API.Services
{
    /// <summary>
    /// Service responsible for operations related to test executions.
    /// </summary>
    public class TestExecutionService
    {
        private readonly TestManagementDbContext _dbContext;

        private readonly ILogger<TestExecutionService>? _logger = null;


        /// <summary>
        /// Constructs a new instance of <see cref="TestExecutionService"/>.
        /// </summary>
        /// <param name="dbContext">Database context used for persistence.</param>
        /// <param name="logger">Logger instance for the service.</param>
        public TestExecutionService(
            TestManagementDbContext dbContext,
            ILogger<TestExecutionService> logger
            )
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new test execution with the specified request data and persists it to the database.
        /// Validates that the environment, test cases and statuses exist before saving.
        /// </summary>
        /// <param name="request">The request DTO containing execution metadata and test case results.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A DTO containing details of the created test execution.</returns>
        public virtual async Task<CreateTestExecutionResponse> CreateAsync(CreateTestExecutionRequest request, CancellationToken ct = default)
        {
            _logger?.LogDebug("TestExecutionService.CreateAsync() start!");

            var isExists = await _dbContext.TestExecutions.AnyAsync(_ => _.Revision == request.Revision);
            if (isExists)
            {
                throw new Exception($"Test execution about {request.Revision} already exists.");
            }

            var testEnvironment = _dbContext.Environments.Where(_ => _.Name == request.Environment).First();
            if (null == testEnvironment)
            {
                throw new Exception($"Test environment {request.Environment} does not exists.");
            }

            var testExecution = new TestExecution()
            {
                Revision = request.Revision,
                ExecutedAt = request.ExecutedAt,
                EnvironmentId = testEnvironment.Id
            };
            var executedTests = new List<TestExecution.ExecutedTest>();
            foreach (var testCaseItem in request.TestCases)
            {
                var testCaseVersions = _dbContext.TestCases
                    .Where(_ => _.Code == testCaseItem.TestCaseCode)
                    .Select(_ => _.Versions)
                    .FirstOrDefault();
                if (null == testCaseVersions)
                {
                    throw new Exception($"Test case about code {testCaseItem.TestCaseCode} does not exists.");
                }
                var testCaseVersionId = testCaseVersions?
                    .Where(_ => _.VersionNumber == testCaseItem.TestCaseVersion)
                    .Select(_ => _.Id)
                    .FirstOrDefault();
                if (0 == testCaseVersionId) 
                {
                    throw new Exception($"Test case about code {testCaseItem.TestCaseCode} and version {testCaseItem.TestCaseVersion} does not exists.");
                }
                var testStatusId = _dbContext.TestStatuses
                    .Where(_ => _.Code == testCaseItem.TestStatusCode)
                    .Select(_ => _.Id)
                    .FirstOrDefault();
                if (0 == testStatusId)
                {
                    throw new Exception($"Test status about code {testCaseItem.TestStatusCode} does not exists.");
                }

                var executedTest = new TestExecution.ExecutedTest()
                {
                    TestCaseVersionId = (long)testCaseVersionId!,
                    TestStatusId = testStatusId
                };
                executedTests.Add(executedTest);
            }
            testExecution.AddExecutionItem(testEnvironment.Id, request.ExecutedAt, executedTests);
            _dbContext.TestExecutions.Add(testExecution);
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception($"Failed to save test execution about revision {request.Revision}.");
            }

            var createdTestExecution = await _dbContext.TestExecutions
                .Where(_ => _.Revision == request.Revision)
                .Include(_ => _.Environment)
                .Include(_ => _.Items)
                    .ThenInclude(i => i.TestResults)
                .FirstOrDefaultAsync();
            var response = new CreateTestExecutionResponse()
            {
                TestExecutionId = createdTestExecution!.Id,
                Environment = request.Environment,
                ExecutedAt = request.ExecutedAt,
                TestCases = request.TestCases
            };

            return response;
        }
    }
}
