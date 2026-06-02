using TestManagement.APP.Dto.TestExecution.Get;
using TestManagement.APP.Dto.TestLevel.Get;

namespace TestManagement.APP.ApiClients.TestLevel
{
    /// <summary>
    /// API client responsible for retrieving test level data from the remote Test API.
    /// </summary>
    public class TestLevelApiClient : ITestLevelApiClient
    {
        /// <summary>
        /// Logger used for diagnostic messages from this client.
        /// </summary>
        private readonly ILogger<TestLevelApiClient> _logger;

        /// <summary>
        /// HttpClient instance configured for the Test API.
        /// </summary>
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Creates a new instance of <see cref="TestLevelApiClient"/>.
        /// </summary>
        /// <param name="logger">Logger instance provided by DI.</param>
        /// <param name="httpClientFactory">Factory used to create an <see cref="HttpClient"/> for the API.</param>
        public TestLevelApiClient(
            ILogger<TestLevelApiClient> logger,
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient("TestApiClient");
        }

        /// <summary>
        /// Retrieves the list of test levels from the remote API.
        /// Returns an empty list if the API returns no data.
        /// </summary>
        /// <returns>A list of <see cref="GetTestLevelResponse"/> objects.</returns>
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
