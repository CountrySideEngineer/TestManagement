using TestManagement.APP.Dto.TestExecution.Get;
using TestManagement.APP.Dto.TestExecution.Post;

namespace TestManagement.APP.ApiClients
{
    /// <summary>
    /// API client for operations related to test executions.
    /// Provides methods to query and create test execution records via the Test API.
    /// </summary>
    public class TestExecutionApiClient : ITestExecutionApiClient
    {
        /// <summary>
        /// Logger for diagnostic and tracing messages.
        /// </summary>
        private readonly ILogger<TestExecutionApiClient> _logger;

        /// <summary>
        /// HttpClient instance configured for the Test API (named client "TestApiClient").
        /// </summary>
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of <see cref="TestExecutionApiClient"/>.
        /// </summary>
        /// <param name="logger">Logger provided by dependency injection.</param>
        /// <param name="httpClientFactory">Factory used to create the HttpClient for API calls.</param>
        public TestExecutionApiClient(
            ILogger<TestExecutionApiClient> logger,
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient("TestApiClient");
        }

        /// <summary>
        /// Retrieves all test executions from the Test API.
        /// Returns an empty list when no executions are available.
        /// </summary>
        /// <returns>A list of <see cref="GetTestExecutionResponse"/> or an empty list.</returns>
        public virtual async Task<IList<GetTestExecutionResponse>?> GetTestExecutionsAsync()
        {
            _logger?.LogDebug("TestExecutionApiClient::GetTestExecutionsAsync() start!");

            var result = await _httpClient!
                .GetFromJsonAsync<List<GetTestExecutionResponse>>("api/TestExecution") ?? 
                    new List<GetTestExecutionResponse>();

            return result;
        }

        /// <summary>
        /// Creates a new test execution by posting <paramref name="request"/> to the API.
        /// Returns the created execution response on success, otherwise <c>null</c> when the request failed.
        /// </summary>
        /// <param name="request">The request payload describing the test execution to create.</param>
        /// <returns>The created <see cref="PostTestExecutionResponse"/> or <c>null</c> on failure.</returns>
        public virtual async Task<PostTestExecutionResponse?> CreateTestExecutionAsync(PostTestExecutionRequest request)
        {
            _logger?.LogDebug("TestExecutionApiClient::CreateTestExecutionAsync() start!");

            var response = await _httpClient.PostAsJsonAsync("api/testexecution", request);

            if (!response.IsSuccessStatusCode)
            {
                _logger?.LogWarning("TestExecutionApiClient::CreateTestExecutionAsync() failed with status {StatusCode}", response.StatusCode);
                return null;
            }

            var result = await response.Content.ReadFromJsonAsync<PostTestExecutionResponse>();

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

        public virtual async Task<GetTestExecutionResponse?> GetTestExecutionsByIdAsync(long id)
        {
            _logger?.LogDebug("TestExecutionApiClient::GetTestExecutionsByIdAsync() start!");

            string url = $"api/testexecution/{id}";

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                _logger?.LogWarning("TestExecutionApiClient::GetTestExecutionsByIdAsync() failed with status {StatusCode} for id {Id}", response.StatusCode, id);
                throw new HttpRequestException($"Failed to get test execution with id {id}. Status code: {response.StatusCode}");
            }
    
            var result = response.Content.ReadFromJsonAsync<GetTestExecutionResponse>().Result;
            if (result == null)
            {
                _logger?.LogWarning("TestExecutionApiClient::GetTestExecutionsByIdAsync() returned null for id {Id}", id);
                throw new InvalidOperationException($"Received null response when fetching test execution with id {id}");
            }
    
            return result;
        }


        //Task<IList<GetTestExecutionResponse>?> ITestExecutionApiClient.GetTestExecutionsAsync()
        //{
        //    throw new NotImplementedException();
        //}

        //Task<PostTestExecutionResponse?> ITestExecutionApiClient.CreateTestExecutionAsync(PostTestExecutionRequest request)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
