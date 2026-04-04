using Microsoft.EntityFrameworkCore;
using System.Formats.Asn1;
using TestManagement.API.Data;
using TestManagement.API.Features.TestCases.Create;
using TestManagement.API.Features.testExecutions.Create;
using TestManagement.API.Features.TestExecutions;
using TestManagement.API.Features.TestExecutions.Create;
using TestManagement.API.Features.TestExecutions.Get;
using TestManagement.API.Features.TestExecutions.Update;
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

        public virtual async Task<List<GetTestExecutionResponse>> GetAsync(CancellationToken ct = default)
        {
            _logger?.LogDebug("TestExecutionService.GetAsync() start!");
            var testExecutions = await _dbContext.TestExecutions
                .Include(_ => _.Environment)
                .Include(_ => _.Items)
                    .ThenInclude(i => i.TestResults)
                        .ThenInclude(i => i.TestCaseVersion)
                            .ThenInclude(tc => tc.TestCase)
                .Include(_ => _.Items)
                    .ThenInclude(_ => _.TestResults)
                        .ThenInclude(i => i.Status) 
                .ToListAsync();
            var response = testExecutions.Select(_ => new GetTestExecutionResponse()
            {
                TestExecutionId = _.Id,
                Environment = _.Environment.Name,
                ExecutedAt = _.ExecutedAt,
                Revision = _.Revision,
                TestCases = _.Items.SelectMany(i => i.TestResults.Select(tr => new TestCaseExecution()
                {
                    TestCaseCode = tr.TestCaseVersion.TestCase!.Code,
                    TestCaseVersion = tr.TestCaseVersion.VersionNumber,
                    TestStatusCode = tr.Status.Code
                })).ToList()
            }).ToList();

            return response;
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

            var testEnvironment = _dbContext.Environments.Where(_ => _.Name == request.Environment).First();
            if (null == testEnvironment)
            {
                throw new Exception($"Test environment {request.Environment} does not exists.");
            }

            var isExists = await _dbContext.TestExecutions
                .AnyAsync(_ => _.Revision == request.Revision && _.EnvironmentId == testEnvironment.Id);
            if (isExists)
            {
                throw new Exception($"Test execution about {request.Revision} already exists.");
            }

            var testExecution = new TestExecution()
            {
                Revision = request.Revision,
                ExecutedAt = request.ExecutedAt,
                EnvironmentId = testEnvironment.Id
            };

            ICollection<TestExecution.ExecutedTest> executedTests = GetExecutedTestsFromRequest(request.TestCases);

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

        /// <summary>
        /// Updates an existing test execution by adding a new execution item.
        /// Validates that the execution, environment, test cases and statuses exist before persisting changes.
        /// </summary>
        /// <param name="request">The request DTO containing the execution metadata and test case results to add.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A DTO containing details of the updated test execution.</returns>
        public virtual async Task<UpdateTestExecutionResponse> UpdateAsync(UpdateTestExecutionRequest request, CancellationToken ct = default)
        {
            _logger?.LogDebug("TestExecutionService.UpdateAsync() start!");

            var testEnvironment = _dbContext.Environments.Where(_ => _.Name == request.Environment).First();
            if (null == testEnvironment)
            {
                throw new Exception($"Test environment {request.Environment} does not exists.");
            }

            var isExists = await _dbContext.TestExecutions
                .AnyAsync(_ => _.Revision == request.Revision && _.EnvironmentId == testEnvironment.Id);
            if (!isExists)
            {
                throw new Exception($"Test execution about {request.Revision} does not exist.");
            }

            var testExecution = await _dbContext.TestExecutions
                .Where(_ => _.Revision == request.Revision && _.EnvironmentId == testEnvironment.Id)
                .Include(_ => _.Items)
                    .ThenInclude(i => i.TestResults)
                .FirstOrDefaultAsync();
            if (null == testExecution)
            {
                throw new Exception($"Test execution about {request.Revision} does not exist.");
            }

            ICollection<TestExecution.ExecutedTest> executedTests = GetExecutedTestsFromRequest(request.TestCases);

            testExecution.AddExecutionItem(testExecution.EnvironmentId, request.ExecutedAt, executedTests);
            await _dbContext.SaveChangesAsync();

            var response = new UpdateTestExecutionResponse()
            {
                TestExecutionId = testExecution.Id,
                Environment = request.Environment,
                ExecutedAt = request.ExecutedAt,
                Revision = request.Revision,
                TestCases = request.TestCases
            };
            return response;
        }

        /// <summary>
        /// Maps a collection of request test case execution DTOs to the domain executed test entities.
        /// </summary>
        /// <param name="testCaseExecutions">List of test case execution DTOs from the request.</param>
        /// <returns>A collection of <see cref="TestExecution.ExecutedTest"/> entities ready to be persisted.</returns>
        protected virtual ICollection<TestExecution.ExecutedTest> GetExecutedTestsFromRequest(List<TestCaseExecution> testCaseExecutions)
        {
            var executedTests = new List<TestExecution.ExecutedTest>();
            foreach (var testCaseItem in testCaseExecutions)
            {
                TestExecution.ExecutedTest executedTest = GetExecutedTestFromExecution(testCaseItem);
                executedTests.Add(executedTest);
            }
            return executedTests;
        }

        /// <summary>
        /// Converts a single request test case execution DTO into a <see cref="TestExecution.ExecutedTest"/> entity.
        /// Validates that the referenced test case version and status exist in the database.
        /// </summary>
        /// <param name="testCaseExecution">The DTO describing a single test case execution.</param>
        /// <returns>The mapped <see cref="TestExecution.ExecutedTest"/> domain entity.</returns>
        /// <exception cref="Exception">Thrown when the referenced test case, version or status does not exist.</exception>
        protected virtual TestExecution.ExecutedTest GetExecutedTestFromExecution(TestCaseExecution testCaseExecution)
        {
            var testCaseVersions = _dbContext.TestCases
                .Where(_ => _.Code == testCaseExecution.TestCaseCode)
                .Select(_ => _.Versions)
                .FirstOrDefault();
            if (null == testCaseVersions)
            {
                throw new Exception($"Test case about code {testCaseExecution.TestCaseCode} does not exists.");
            }
            var testCaseVersionId = testCaseVersions?
                .Where(_ => _.VersionNumber == testCaseExecution.TestCaseVersion)
                .Select(_ => _.Id)
                .FirstOrDefault();
            if (0 == testCaseVersionId)
            {
                throw new Exception($"Test case about code {testCaseExecution.TestCaseCode} and version {testCaseExecution.TestCaseVersion} does not exists.");
            }
            var testStatusId = _dbContext.TestStatuses
                .Where(_ => _.Code == testCaseExecution.TestStatusCode)
                .Select(_ => _.Id)
                .FirstOrDefault();
            if (0 == testStatusId)
            {
                throw new Exception($"Test status about code {testCaseExecution.TestStatusCode} does not exists.");
            }
            var executedTest = new TestExecution.ExecutedTest()
            {
                TestCaseVersionId = (long)testCaseVersionId!,
                TestStatusId = testStatusId
            };

            return executedTest;
        }
    }
}
