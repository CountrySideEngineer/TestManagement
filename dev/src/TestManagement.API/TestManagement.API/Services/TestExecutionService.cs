using Microsoft.EntityFrameworkCore;
using TestManagement.API.Data;
using TestManagement.API.Features.TestCases.Create;
using TestManagement.API.Features.testExecutions.Create;
using TestManagement.API.Features.TestExecutions.Create;
using TestManagement.API.Models;

namespace TestManagement.API.Services
{
    public class TestExecutionService
    {
        private readonly TestManagementDbContext _dbContext;

        private readonly ILogger<TestExecutionService>? _logger = null;


        public TestExecutionService(
            TestManagementDbContext dbContext,
            ILogger<TestExecutionService> logger
            )
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public virtual async Task<CreateTestExecutionResponse> CreateAsync(CreateTestExecutionRequest request, CancellationToken ct = default)
        {
            _logger?.LogDebug("TestExecutionService.CreateAsync() start!");

            var isExists = await _dbContext.TestExecutions.AnyAsync(_ => _.Revision == request.Revisoin);
            if (isExists)
            {
                throw new Exception($"Test execution about {request.Revisoin} already exists.");
            }

            var testEnvironment = _dbContext.Environments.Where(_ => _.Name == request.Environment).First();
            if (null == testEnvironment)
            {
                throw new Exception($"Test environment {request.Environment} does not exists.");
            }

            var testExecution = new TestExecution()
            {
                Revision = request.Revisoin,
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
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception($"Failed to save test execution about revision {request.Revisoin}.");
            }

            var createdTestExecution = await _dbContext.TestExecutions
                .Where(_ => _.Revision == request.Revisoin)
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
