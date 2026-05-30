using TestManagement.APP.ApiClients.TestCase;
using TestManagement.APP.Dto.TestCase.Post;
using TestManagement.APP.Dto.TestResult;
using TestManagement.APP.ViewModel;

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
