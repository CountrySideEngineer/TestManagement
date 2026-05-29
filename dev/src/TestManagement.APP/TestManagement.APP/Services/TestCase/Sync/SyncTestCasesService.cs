using TestManagement.APP.ApiClients.TestCase;
using TestManagement.APP.Dto.TestCase.Post;
using TestManagement.APP.Dto.TestResult;

namespace TestManagement.APP.Services.TestCase.Sync
{
    public class SyncTestCasesService : ISyncTestCasesService
    {
        private readonly ILogger<SyncTestCasesService> _logger;
        private readonly ITestCaseSyncApiClient _apiClient;

        public SyncTestCasesService(
            ILogger<SyncTestCasesService> logger, 
            ITestCaseSyncApiClient apiClient)
        {
            _logger = logger;
            _apiClient = apiClient;
        }

        public async Task SyncTestCasesAsync(IEnumerable<ParsedTestResult> testResults)
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

            var result = await _apiClient.SyncAsync(requests);

            return;
        }
    }
}
