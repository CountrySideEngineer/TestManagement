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
            List<TestRunDto>? testRuns = await _httpClient.GetFromJsonAsync<List<TestRunDto>>("api/testrun");
            if (null == testRuns)
            {
                return new List<TestRunDto>();

            }
            List<TestResultDto>? testResults = await _httpClient.GetFromJsonAsync<List<TestResultDto>>("api/testresult");
            if (null == testResults)
            {
                return testRuns;
            }
            foreach (var item in testRuns)
            {
                var testResult = testResults.Where(_ => _.TestRunId == item.Id).ToList();
                item.TestResults = testResult;
            }

            return testRuns;
        }
    }
}
