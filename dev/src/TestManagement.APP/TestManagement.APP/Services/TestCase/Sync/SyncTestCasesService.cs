using TestManagement.APP.ApiClients.TestCase;
using TestManagement.APP.Dto.TestCase.Post;
using TestManagement.APP.Dto.TestResult;
using TestManagement.APP.ViewModel;

namespace TestManagement.APP.Services.TestCase.Sync
{
    /// <summary>
    /// Service for synchronizing test cases with the database.
    /// Ensures that test cases from parsed results exist in the database and retrieves them as view models.
    /// </summary>
    public class SyncTestCasesService : ISyncTestCasesService
    {
        /// <summary>
        /// Logger for recording diagnostic information and errors.
        /// </summary>
        private readonly ILogger<SyncTestCasesService> _logger;

        /// <summary>
        /// API client for synchronizing test cases with the backend service.
        /// </summary>
        private readonly ITestCaseSyncApiClient _apiClient;

        /// <summary>
        /// Constructs an instance of <see cref="SyncTestCasesService"/>.
        /// </summary>
        /// <param name="logger">Logger for diagnostics.</param>
        /// <param name="apiClient">API client for test case synchronization.</param>
        public SyncTestCasesService(
            ILogger<SyncTestCasesService> logger, 
            ITestCaseSyncApiClient apiClient)
        {
            _logger = logger;
            _apiClient = apiClient;
        }

        /// <summary>
        /// Synchronizes parsed test results with the database asynchronously.
        /// Converts parsed test results to API requests, syncs them, and returns the corresponding view models.
        /// </summary>
        /// <param name="testResults">Collection of parsed test results to synchronize.</param>
        /// <returns>An enumerable of <see cref="TestCaseViewModel"/> representing the synchronized test cases.</returns>
        public async Task<IEnumerable<TestCaseViewModel>> SyncTestCasesAsync(IEnumerable<ParsedTestResult> testResults)
        {
            _logger.LogDebug("SyncTestCasesService::SyncTestCasesAsync() start!");

            var requests = new List<PostTestCaseRequest>();
            foreach (var syncTestCaseRequest in testResults)
            {
                var request = new PostTestCaseRequest
                {
                    Code = syncTestCaseRequest.Code,
                    Name = syncTestCaseRequest.Name,
                    Description = syncTestCaseRequest.Description,
                    TestLevelId = 1
                };
                requests.Add(request);
            }

            IEnumerable<PostTestCaseResponse> responses = await _apiClient.SyncAsync(requests);
            var viewModels = responses.Select(r => new TestCaseViewModel
            {
                Id = r.Id,
                Code = r.Code,
                Name = r.Name,
                Description = r.Description,
                TestLevelId = r.TestLevelId,
                VersionNumber = r.VersionNumber
            });

            return viewModels;
        }
    }
}
