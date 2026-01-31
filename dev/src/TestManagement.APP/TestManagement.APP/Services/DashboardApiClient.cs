using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json;
using TestManagement.APP.Models;

namespace TestManagement.APP.Services
{
    public class DashboardApiClient
    {
        private readonly HttpClient _httpClient;

        public DashboardApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("TestApiClient");
        }

        public async Task<SummaryDto> GetSummaryAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<Models.TestRunDto>>("api/testrun/");
                var latestRun = response?.OrderByDescending(_ => _.ExecutedAt).First().Id;
                var testResults = await _httpClient.GetFromJsonAsync<List<Models.TestResultDto>>($"api/testresult/");
                var testRunResults = testResults?.Where(_ => _.TestRunId == latestRun).ToList();
                int execNum = null == testRunResults ? 0 : testRunResults.Count();
                int errNum = null == testRunResults ? 0 : testRunResults.Where(_ => _.Status == TestStatus.Failure).Count();
                int skippedNum = null == testResults ? 0 : testResults.Where(_ => _.Status == TestStatus.Skipped).Count();
                int disabledNum = null == testResults ? 0 : testResults.Where(_ => _.Status == TestStatus.Blocked).Count();
                var summary = new SummaryDto(errNum, skippedNum, disabledNum, execNum);

                return summary;
            }
            catch (Exception)
            {
                return new SummaryDto(0, 0, 0, 0);
            }
        }

        public async Task<TestRunDto> GetLatestTestRunAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<Models.TestRunDto>>("api/testrun/");
            try
            {
                var latestRun = response?.OrderByDescending(_ => _.ExecutedAt).First();

                return (null == latestRun ? new TestRunDto() : latestRun);
            }
            catch (Exception ex)
            when ((ex is ArgumentNullException) || (ex is InvalidOperationException))
            {
                return new TestRunDto();
            }
        }

        public async Task<List<Models.TestResultDto>> GetTestRecordsByTestRunAsync(int testRunId)
        {
            try
            {
                var testResults = await _httpClient.GetFromJsonAsync<List<Models.TestResultDto>>($"api/testresult/");
                var testRunResults = testResults?.Where(_ => _.TestRunId == testRunId).ToList();

                return (null == testRunResults ? new List<Models.TestResultDto>() : testRunResults);
            }
            catch (Exception)
            {
                return new List<TestResultDto>();
            }
        }
    }
}
