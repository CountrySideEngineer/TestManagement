using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
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

        /// <summary>
        /// Creates a new test run via the API.
        /// Returns the created <see cref="TestRunDto"/> returned by the server on success; otherwise returns null.
        /// </summary>
        /// <param name="newRun">The test run to create.</param>
        /// <returns>The created <see cref="TestRunDto"/> on success, or null on failure.</returns>
        public async Task<TestRunDto?> CreateTestRunAsync(TestRunDto newRun)
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
    }
}
