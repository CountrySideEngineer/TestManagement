using Microsoft.EntityFrameworkCore.Storage;
using TestManagement.APP.Dto.TestExecution.Get;

namespace TestManagement.APP.ApiClients
{
    public class TestExecutionApiClient : ITestExecutionApiClient
    {
        private readonly ILogger<TestExecutionApiClient> _logger;
        private readonly HttpClient _httpClient;

        public TestExecutionApiClient(
            ILogger<TestExecutionApiClient> logger,
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient("TestApiClient");
        }

        public virtual async Task<IList<GetTestExecutionResponse>?> GetTestExecutionsAsync()
        {
            _logger?.LogDebug("TestExecutionApiClient::GetTestExecutionsAsync() start!");

            var result = await _httpClient!
                .GetFromJsonAsync<List<GetTestExecutionResponse>>("api/testexecution") ?? 
                    new List<GetTestExecutionResponse>();

            return result;
        }
    }
}
