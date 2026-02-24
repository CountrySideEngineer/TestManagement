using TestManagement.APP.Models;

namespace TestManagement.APP.Services
{
    public class UploadApiClient
    {
        private readonly HttpClient _httpClient;

        public UploadApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("TestApiClient");
        }

        public async Task<List<TestLevelDto>> GetTestLevelAsync()
        {
            var testLevels = await _httpClient.GetFromJsonAsync<List<TestLevelDto>>("api/testlevels");
            if (null == testLevels)
            {
                return new List<TestLevelDto>();
            }
            else
            {
                return testLevels;
            }
        }
    }
}
