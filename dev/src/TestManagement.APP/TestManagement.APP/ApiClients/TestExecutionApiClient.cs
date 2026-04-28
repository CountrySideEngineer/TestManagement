using Microsoft.EntityFrameworkCore.Storage;
using TestManagement.APP.Dto.TestExecution.Get;
using TestManagement.APP.Dto.TestExecution.Post;

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

        public virtual async Task<global::TestManagement.APP.Dto.TestExecution.Post.PostTestExecutionResponse?> CreateTestExecutionAsync(global::TestManagement.APP.Dto.TestExecution.Post.PostTestExecutionRequest request)
        {
            _logger?.LogDebug("TestExecutionApiClient::CreateTestExecutionAsync() start!");

            var response = await _httpClient.PostAsJsonAsync("api/testexecution", request);

            if (!response.IsSuccessStatusCode)
            {
                _logger?.LogWarning("TestExecutionApiClient::CreateTestExecutionAsync() failed with status {StatusCode}", response.StatusCode);
                return null;
            }

            var result = await response.Content.ReadFromJsonAsync<global::TestManagement.APP.Dto.TestExecution.Post.PostTestExecutionResponse>();

            if (result != null && result.CreatedExecution != null)
            {
                _logger?.LogDebug("TestExecutionApiClient::CreateTestExecutionAsync() created id: {Id}, executed at: {ExecutedAt}, environment: {Environment}, revision: {Revision}",
                    result.CreatedExecution.TestExecutionId,
                    result.CreatedExecution.ExecutedAt,
                    result.CreatedExecution.Environment,
                    result.CreatedExecution.Revision);
            }

            return result;
        }
    }
}
