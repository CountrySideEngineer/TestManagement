using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Eventing.Reader;
using TestManagement.API.Data;
using TestManagement.API.Data.Repositories;
using TestManagement.API.Features.TestResult.Create;
using TestManagement.API.Features.TestResult.Get;
using TestManagement.API.Models;
using TestManagement.API.Models.Report.Xml;
using TestManagement.API.Services.Xml;

namespace TestManagement.API.Services
{
    public class TestResultService
    {
        /// <summary>
        /// Service responsible for handling operations related to test results,
        /// including creation, retrieval and mapping to response DTOs.
        /// </summary>
        private readonly ITestResultRepository _testResultRepository;

        /// <summary>
        /// Repository used to access and manipulate test result entities.
        /// </summary>
        private readonly ITestResultXmlConverter _xmlConverter;

        /// <summary>
        /// Converter responsible for transforming test result XML data to domain models and vice versa.
        /// </summary>

        /// <summary>
        /// Database context used to access and persist test execution related entities.
        /// </summary>
        private readonly TestManagementDbContext _dbContext;

        /// <summary>
        /// Optional logger instance for diagnostic logging within the service.
        /// </summary>
        private readonly ILogger<TestResultService> _logger;

        // Constructor initializes required dependencies for the service.
        /// <summary>
        /// Initializes a new instance of the <see cref="TestResultService"/> class.
        /// </summary>
        /// <param name="dbContext">Database context for accessing persistence.</param>
        /// <param name="testResultRepository">Repository for test result entities.</param>
        /// <param name="xmlConverter">XML converter for test reports.</param>
        /// <param name="logger">Logger instance for diagnostic messages.</param>
        public TestResultService(
            TestManagementDbContext dbContext,
            ITestResultRepository testResultRepository,
            ITestResultXmlConverter xmlConverter,
            ILogger<TestResultService> logger
            )
        {
            _dbContext = dbContext;
            _testResultRepository = testResultRepository;
            _xmlConverter = xmlConverter;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all test results along with their related entities and returns them
        /// as a collection of <see cref="GetTestResultResponse"/> DTOs.
        /// </summary>
        /// <returns>Collection of test result response DTOs.</returns>
        public async Task<ICollection<GetTestResultResponse>> GetAllAsync()
        {
            _logger.LogDebug("TestResultService::GetAllAsync() start!");

            var testResults = await _dbContext.TestResults
                .Include(_ => _.TestCaseVersion)
                    .ThenInclude(_ => _.TestLevel)
                .Include(_ => _.Status)
                .ToListAsync();

            // Map domain models to response DTOs. Mapping is delegated to a private helper
            // to keep GetAllAsync concise and make mapping testable in isolation.
            var responses = testResults.Select(MapToResponse).ToList();

            _logger.LogDebug("TestResultService::GetAllAsync() finished. Returning {Count} results.", responses.Count);

            return responses;
        }

        /// <summary>
        /// Retrieves a test result domain model by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the test result.</param>
        /// <returns>The test result domain model, or null if not found.</returns>
        public async Task<Models.TestResult> GetByIdAsync(int id)
        {
            _logger.LogDebug("TestResultService::GetByIdAsync() start!");

            return await _testResultRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// Creates a new test result entity from the provided request and persists it to the database.
        /// </summary>
        /// <param name="request">The request containing test result data.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task<CreateTestResultResponse> CreateAsync(CreateTestResultRequest request)
        {
            _logger.LogDebug("TestResultService::Create() start!");

            var testResult = new Models.TestResult();
            testResult.TestExecutionItemId = request.TestExecutionItemId;
            testResult.TestCaseVersionId = await _dbContext.TestCaseVersions
                .Where(_ =>
                    _.TestCaseId == request.TestCaseId &&
                    _.VersionNumber == request.TestCaseVersionNumber)
                .Select(_ => _.Id)
                .FirstOrDefaultAsync();
            testResult.StatusId = await _dbContext.TestStatuses
                .Where(_ => 
                    _.Code.ToLower() == request.TestResultStatus.ToLower())
                .Select(_ => _.Id)
                .FirstOrDefaultAsync();
            testResult.Message = request.Message;
            testResult.ExecutedAt = request.ExecutedAt;

            _dbContext.TestResults.Add(testResult);

            await _dbContext.SaveChangesAsync();

            var response = new CreateTestResultResponse()
            {
                ResultId = testResult.Id,
                TestExecutionItemId = testResult.TestExecutionItemId,
                TestCaseId = request.TestCaseId,
                TestCaseVersionNumber = request.TestCaseVersionNumber,
                TestLevelId = request.TestLevelId,
                ExecutedAt = request.ExecutedAt,
                Message = request.Message,
                TestResultStatus = request.TestResultStatus
            };

            return response;
        }

        /// <summary>
        /// Maps a domain <see cref="Models.TestResult"/> instance to a <see cref="GetTestResultResponse"/> DTO.
        /// </summary>
        /// <param name="tr">The domain test result to map.</param>
        /// <returns>The mapped response DTO.</returns>
        private static GetTestResultResponse MapToResponse(Models.TestResult tr)
        {
            return new GetTestResultResponse
            {
                Id = tr.Id,
                TestExecutionItemId = tr.TestExecutionItemId,
                TestCaseVersionId = tr.TestCaseVersionId,
                TestCaseId = tr.TestCaseVersion?.TestCaseId ?? 0,
                TestCaseVersionName = tr.TestCaseVersion?.Name ?? string.Empty,
                TestCaseVersionNumber = tr.TestCaseVersion?.VersionNumber ?? 0,
                TestLevelId = tr.TestCaseVersion?.TestLevel?.Id,
                TestLevelName = tr.TestCaseVersion?.TestLevel?.Name ?? string.Empty,
                TestLevelCode = tr.TestCaseVersion?.TestLevel?.Code ?? string.Empty,
                StatusCode = tr.Status?.Code ?? string.Empty,
                StatusDisplayName = tr.Status?.DisplayName ?? string.Empty,
                Message = tr.Message,
                ActualResult = tr.ActualResult ?? string.Empty,
                ExecutedAt = tr.ExecutedAt,
                CreatedAt = tr.CreatedAt,
                UpdatedAt = tr.UpdatedAt
            };
        }

        /// <summary>
        /// Creates multiple test result entities from a collection of requests and persists them to the database.
        /// </summary>
        /// <param name="requests">Collection of test result creation requests.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task<ICollection<CreateTestResultResponse>> CreateAsync(ICollection<CreateTestResultRequest> requests)
        {
            _logger.LogDebug("TestResultService::Create() start!");

            var testResults = new List<Models.TestResult>();
            foreach (var requestItem in requests)
            {
                var testResult = new Models.TestResult();
                testResult.TestExecutionItemId = requestItem.TestExecutionItemId;
                testResult.TestCaseVersionId = await _dbContext.TestCaseVersions
                    .Where(_ =>
                        _.TestCaseId == requestItem.TestCaseId &&
                        _.VersionNumber == requestItem.TestCaseVersionNumber)
                    .Select(_ => _.Id)
                    .FirstOrDefaultAsync();
                testResult.StatusId = await _dbContext.TestStatuses
                    .Where(_ =>
                        _.Code.ToLower() == requestItem.TestResultStatus.ToLower())
                    .Select(_ => _.Id)
                    .FirstOrDefaultAsync();
                testResult.Message = requestItem.Message;
                testResult.ExecutedAt = requestItem.ExecutedAt;

                _dbContext.TestResults.Add(testResult);
                testResults.Add(testResult);
            }

            await _dbContext.SaveChangesAsync();

            var responses = new List<CreateTestResultResponse>();
            foreach (var testResultItem in testResults)
            {
                string testResultStatus = await _dbContext.TestStatuses
                    .Where(_ => _.Id == testResultItem.StatusId)
                    .Select(_ => _.Code)
                    .FirstOrDefaultAsync() ?? string.Empty;
                var testCaseVersion = await _dbContext.TestCaseVersions
                    .Where(_ => _.Id == testResultItem.TestCaseVersionId)
                    .FirstOrDefaultAsync();
                long testCaseId = testCaseVersion?.TestCaseId ?? 0;
                long versionNumber = testCaseVersion?.VersionNumber ?? 0;
                long testLevelId = testCaseVersion?.TestLevelId ?? 0;

                var response = new CreateTestResultResponse()
                {
                    ResultId = testResultItem.Id,
                    TestExecutionItemId = testResultItem.TestExecutionItemId,
                    TestCaseId = testCaseId,
                    TestCaseVersionNumber = versionNumber,
                    TestLevelId = testLevelId,
                    ExecutedAt = testResultItem.ExecutedAt,
                    Message = testResultItem.Message,
                    TestResultStatus = testResultStatus
                };
                responses.Add(response);
            }

            return responses;
        }
    }
}
