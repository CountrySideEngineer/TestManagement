using TestManagement.APP.Models;

namespace TestManagement.APP.Services
{
    public class TestRunApiClient
    {
        private readonly HttpClient _httpClient;

        public TestRunApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("TestApiClient");
        }

        public async Task<List<TestRunDto>> GetTestRunsAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<TestRunDto>>("api/TestRun");

            return response;
        }
    }
}
