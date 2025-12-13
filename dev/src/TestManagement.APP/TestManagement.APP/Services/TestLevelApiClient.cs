using TestManagement.APP.Models;

namespace TestManagement.APP.Services
{
    public class TestLevelApiClient
    {
        private readonly HttpClient _httpClient;

        public TestLevelApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("TestApiClient");
        }

        public async Task<List<Models.TestLevelDto>> GetTestLevelsAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<TestLevelDto>>("api/testlevel");
            return response;
        }
    }
}
