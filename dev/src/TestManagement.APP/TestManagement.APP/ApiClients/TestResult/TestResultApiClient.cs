using TestManagement.APP.Dto.TestCase.Post;

namespace TestManagement.APP.ApiClients.TestResult
{
    public class TestResultApiClient : ITestResultApiClient
    {
        private readonly ILogger<TestResultApiClient> _logger;

        private readonly HttpClient _httpClient;

        public TestResultApiClient(
            ILogger<TestResultApiClient> logger,
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient("TestApiClient");
        }

        public async Task<IEnumerable<PostTestCaseResponse>> CreateTestResultAsync(IEnumerable<PostTestCaseRequest> requests, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Creating test results with {Count} requests", requests.Count());

            var response = await _httpClient
                .PostAsJsonAsync("/api/testresult/bulk", requests, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to create test results. Status Code: {StatusCode}", response.StatusCode);
                throw new HttpRequestException($"Failed to create test results. Status Code: {response.StatusCode}");
            }
            else
            {
                var result = await response.Content.ReadFromJsonAsync<IEnumerable<PostTestCaseResponse>>(cancellationToken: cancellationToken);

                return result;
            }
        }
    }
}
