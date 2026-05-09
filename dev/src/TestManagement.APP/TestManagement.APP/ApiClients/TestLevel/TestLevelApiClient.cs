using TestManagement.APP.Dto.TestExecution.Get;
using TestManagement.APP.Dto.TestLevel.Get;

namespace TestManagement.APP.ApiClients.TestLevel
{
    public class TestLevelApiClient : ITestLevelApiClient
    {
        private readonly ILogger<TestLevelApiClient> _logger;

        private readonly HttpClient _httpClient;

        public TestLevelApiClient(
            ILogger<TestLevelApiClient> logger, 
            HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<IList<GetTestLevelResponse>> GetTestLevelAsync()
        {
            _logger.LogInformation("TestLevelApiClient::GetTestLevelAsync");

            var responses = await _httpClient
                .GetFromJsonAsync<List<GetTestLevelResponse>>("api/TestLevel") ??
                    new List<GetTestLevelResponse>();

            return responses;
        }
    }
}
