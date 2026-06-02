using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json;
using TestManagement.APP.Models;

namespace TestManagement.APP.Services
{
    /// <summary>
    /// Lightweight API client used by the dashboard to query test-run and test-result data.
    /// Wraps an <see cref="HttpClient"/> configured for the Test API.
    /// </summary>
    public class DashboardApiClient
    {
        /// <summary>
        /// HttpClient instance configured via DI (named client "TestApiClient").
        /// </summary>
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Creates a new <see cref="DashboardApiClient"/> using the provided factory.
        /// </summary>
        /// <param name="httpClientFactory">Factory used to create the configured HttpClient.</param>
        public DashboardApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("TestApiClient");
        }

        /// <summary>
        /// Retrieves a summary of the latest test run including failure/skipped/disabled counts.
        /// Returns a zeroed summary on error.
        /// </summary>
        /// <returns>A <see cref="SummaryDto"/> describing the latest test run statistics.</returns>
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

        /// <summary>
        /// Retrieves the latest test run DTO or an empty <see cref="TestRunDto"/> when none are available.
        /// </summary>
        /// <returns>The latest <see cref="TestRunDto"/>, or a default instance.</returns>
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

        /// <summary>
        /// Retrieves test result records for the specified test run id. Returns an empty list on error.
        /// </summary>
        /// <param name="testRunId">Identifier of the test run to filter results by.</param>
        /// <returns>List of <see cref="TestResultDto"/> for the given run, or an empty list.</returns>
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
