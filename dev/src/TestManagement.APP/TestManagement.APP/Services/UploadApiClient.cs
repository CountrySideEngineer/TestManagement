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

        public virtual async Task<List<TestLevelDto>> GetTestLevelAsync()
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

        public virtual async Task<List<TestRunDto>> GetLatestTestRunAsync()
        {
            var testRuns = await _httpClient.GetFromJsonAsync<List<TestRunDto>>("api/testrun/");
            if (null == testRuns)
            {
                return new List<TestRunDto>();
            }

            List<TestResultDto>? testResults = await _httpClient.GetFromJsonAsync<List<TestResultDto>>("api/testresult");
            if (null == testResults)
            {
                return testRuns;
            }

            foreach (var testRun in testRuns)
            {
                testRun.TestResults = testResults.Where(_ => _.TestRunId == testRun.Id).ToList();
            }

            return testRuns;
        }

        public virtual async Task<TestRunDto?> CreateTestRunAsync(TestRunDto newRun)
        {
            if (newRun == null) throw new ArgumentNullException(nameof(newRun));

            var response = await _httpClient.PostAsJsonAsync("api/TestRun", newRun);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var created = await response.Content.ReadFromJsonAsync<TestRunDto>();
            return created;
        }

        public virtual async Task<List<TestCaseDto>> CreateTestCaseAsync(List<TestCaseDto> testCases)
        {
            var response = await _httpClient.PostAsJsonAsync("api/TestCase", testCases);
            if (!response.IsSuccessStatusCode)
            {
                return new List<TestCaseDto>();
            }
            var created = await response.Content.ReadFromJsonAsync<List<TestCaseDto>>();
            return created ?? new List<TestCaseDto>();
        }
    }
}
