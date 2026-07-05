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
    public class TestResultService : ITestResultService
    {
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
            ITestResultXmlConverter xmlConverter,
            ILogger<TestResultService> logger
            )
        {
            _dbContext = dbContext;
            _xmlConverter = xmlConverter;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all test results along with their related entities and returns them
        /// as a collection of <see cref="GetTestResultResponse"/> DTOs.
        /// </summary>
        /// <returns>Collection of test result response DTOs.</returns>
        public async Task<ICollection<GetTestResultResponse>> GetAllAsync(CancellationToken ct)
        {
            _logger.LogDebug("TestResultService::GetAllAsync() start!");

            var testResults = await _dbContext.TestResults
                .Include(_ => _.TestCaseVersion)
                    .ThenInclude(_ => _.TestLevel)
                .Include(_ => _.Status)
                .ToListAsync(ct);

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
        public async Task<GetTestResultResponse> GetByIdAsync(int id, CancellationToken ct)
        {
            _logger.LogDebug("TestResultService::GetByIdAsync() start!");

            Models.TestResult testResult = await _dbContext.TestResults
                .Where(_ => _.Id == id)
                .Include(_ => _.TestCaseVersion)
                .FirstAsync(ct);


            // Map domain models to response DTOs. Mapping is delegated to a private helper
            // to keep GetAllAsync concise and make mapping testable in isolation.
            GetTestResultResponse response = MapToResponse(testResult);

            return response;
        }

        /// <summary>
        /// Creates a new test result entity from the provided request and persists it to the database.
        /// </summary>
        /// <param name="request">The request containing test result data.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task<CreateTestResultResponse> CreateAsync(CreateTestResultRequest request, CancellationToken ct)
        {
            _logger.LogDebug("TestResultService::Create() start!");

            var testResult = new Models.TestResult();
            testResult.TestExecutionItemId = request.TestExecutionItemId;
            testResult.TestCaseVersionId = await _dbContext.TestCaseVersions
                .Where(_ =>
                    _.TestCaseId == request.TestCaseId &&
                    _.VersionNumber == request.TestCaseVersionNumber)
                .Select(_ => _.Id)
                .FirstOrDefaultAsync(ct);
            testResult.StatusId = await _dbContext.TestStatuses
                .Where(_ => 
                    _.Code.ToLower() == request.TestResultStatus.ToLower())
                .Select(_ => _.Id)
                .FirstOrDefaultAsync(ct);
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
        public async Task<ICollection<CreateTestResultResponse>> CreateAsync(ICollection<CreateTestResultRequest> requests, CancellationToken ct)
        {
            _logger.LogDebug("TestResultService::Create() start!");

            // Check arguments.
            if ((requests is null) || (0 == requests.Count))
            {
                return new List<CreateTestResultResponse>();
            }

            var distinctTestCaseKeys = requests
                .Select(_ => new { _.TestCaseId, _.TestCaseVersionNumber })
                .Distinct()
                .ToList();

            var testCaseVersions = await _dbContext.TestCaseVersions
                .Where(_ => distinctTestCaseKeys
                    .Any(key => (key.TestCaseId == _.TestCaseId) && (key.TestCaseVersionNumber == _.VersionNumber)))
                .ToListAsync(ct);

            var testStatusCodes = requests
                .Select(_ => _.TestResultStatus?.ToLowerInvariant() ?? string.Empty)
                .Where(_ => !string.IsNullOrEmpty(_))
                .Distinct()
                .ToList();

            var testStatuses = await _dbContext.TestStatuses
                .Where(s => testStatusCodes.Contains(s.Code.ToLower()))
                .ToListAsync(ct);

            var testCaseVersionMap = testCaseVersions.ToDictionary(
                tc => (tc.TestCaseId, tc.VersionNumber),
                tc => tc.Id);

            var testStatusMap = testStatuses.ToDictionary(
                s => s.Code.ToLowerInvariant(),
                s => s.Id);

            var entities = new List<Models.TestResult>();
            foreach (var req in requests)
            {
                // Validate/look up
                testCaseVersionMap.TryGetValue((req.TestCaseId, req.TestCaseVersionNumber), out var tcVersionId);
                testStatusMap.TryGetValue(req.TestResultStatus?.ToLowerInvariant() ?? string.Empty, out var statusId);

                var tr = new Models.TestResult
                {
                    TestExecutionItemId = req.TestExecutionItemId,
                    TestCaseVersionId = tcVersionId,
                    StatusId = statusId,
                    Message = req.Message,
                    ExecutedAt = req.ExecutedAt
                };

                entities.Add(tr);
            }

            await using var transaction = await _dbContext.Database.BeginTransactionAsync(ct);
            try
            {
                _dbContext.TestResults.AddRange(entities);
                await _dbContext.SaveChangesAsync(ct);

                await transaction.CommitAsync(ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Atomic bulk insert failed, transaction will be rolled back.");
                // transaction disposed without commit -> rollback
                throw;
            }

            // Build responses
            var responses = new List<CreateTestResultResponse>(entities.Count);
            foreach (var ent in entities)
            {
                // Lookup status code and test case version again if needed
                string statusCode = await _dbContext.TestStatuses
                    .Where(s => s.Id == ent.StatusId)
                    .Select(s => s.Code)
                    .FirstOrDefaultAsync(ct) ?? string.Empty;

                var testCaseVersion = await _dbContext.TestCaseVersions
                    .Where(tv => tv.Id == ent.TestCaseVersionId)
                    .FirstOrDefaultAsync(ct);

                long testCaseId = testCaseVersion?.TestCaseId ?? 0;
                long versionNumber = testCaseVersion?.VersionNumber ?? 0;
                long testLevelId = testCaseVersion?.TestLevelId ?? 0;

                responses.Add(new CreateTestResultResponse
                {
                    ResultId = ent.Id,
                    TestExecutionItemId = ent.TestExecutionItemId,
                    TestCaseId = testCaseId,
                    TestCaseVersionNumber = versionNumber,
                    TestLevelId = testLevelId,
                    ExecutedAt = ent.ExecutedAt,
                    Message = ent.Message,
                    TestResultStatus = statusCode
                });
            }

            return responses;
        }
    }
}
