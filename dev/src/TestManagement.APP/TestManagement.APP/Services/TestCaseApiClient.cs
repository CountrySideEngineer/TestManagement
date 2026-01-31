using TestManagement.APP.Models;

namespace TestManagement.APP.Services
{
    public class TestCaseApiClient
    {
        private readonly HttpClient _httpClient;

        public TestCaseApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("TestApiClient");
        }

        public async Task<List<Models.TestCaseDto>> GetTestCaseAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<TestCaseDto>>("api/TestCase");
            return response;
        }
    }
}
