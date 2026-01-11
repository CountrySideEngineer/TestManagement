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
            var testRuns = await _httpClient.GetFromJsonAsync<List<TestRunDto>>("api/testrun");
            var testResults = await _httpClient.GetFromJsonAsync<List<TestResultDto>>("api/testresult");
            foreach (var item in testRuns)
            {
                Console.WriteLine(item.Id);

                var testResult = testResults.Where(_ => _.TestRunId == item.Id).ToList();
                item.TestResults = testResult;
            }

            return testRuns;
        }
    }
}
