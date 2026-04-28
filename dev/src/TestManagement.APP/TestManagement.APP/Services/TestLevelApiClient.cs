using TestManagement.APP.Models;

namespace TestManagement.APP.Services
{
    /// <summary>
    /// HTTP API client used to retrieve test level data from the backend Test API.
    /// This client is a thin wrapper around an <see cref="HttpClient"/> configured by DI.
    /// </summary>
    public class TestLevelApiClient
    {
        /// <summary>
        /// HttpClient instance configured for the Test API (named client "TestApiClient").
        /// </summary>
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Constructs a new <see cref="TestLevelApiClient"/> using the provided factory.
        /// </summary>
        /// <param name="httpClientFactory">Factory used to create the configured <see cref="HttpClient"/>.</param>
        public TestLevelApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("TestApiClient");
        }

        /// <summary>
        /// Retrieves the list of test levels from the API.
        /// </summary>
        /// <returns>A list of <see cref="Models.TestLevelDto"/> objects. The method may throw if the HTTP call fails.</returns>
        public async Task<List<Models.TestLevelDto>> GetTestLevelsAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<TestLevelDto>>("api/testlevel");
            return response!;
        }
    }
}
