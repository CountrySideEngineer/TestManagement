using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using TestManagement.APP.Models;

namespace TestManagement.APP.Services
{
    public class UploadApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<UploadApiClient> _logger;

        public UploadApiClient(ILogger<UploadApiClient> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
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
            _logger.LogInformation("Getting all test runs");

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

        public virtual async Task<TestRunDto> CreateTestRunAsync(TestRunDto newRun)
        {
            _logger.LogInformation("CreateTestRunAsync start!");

            if (newRun == null) throw new ArgumentNullException(nameof(newRun));

            _logger.LogInformation($"{nameof(newRun)}.{nameof(newRun.Abstract)} = {newRun.Abstract}");

            var response = await _httpClient.PostAsJsonAsync("api/TestRun", newRun);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }
            TestRunDto? created = await response.Content.ReadFromJsonAsync<TestRunDto>();
            if (null == created)
            {
                throw new Exception();
            }
            return created;
        }

        public virtual async Task<IActionResult> CreateTestCaseAsync(IList<TestCaseDto> testCases)
        {
            _logger.LogInformation("CreateTestCaseAsync start!");
            _logger.LogInformation($"{nameof(testCases)}.{nameof(testCases.Count)} = {testCases.Count}");

            List<TestCaseDto>? testCasesInDb = await _httpClient.GetFromJsonAsync<List<TestCaseDto>>("api/TestCase");
            List<TestCaseDto> uniqueTestCases = new List<TestCaseDto>();
            if (null != testCasesInDb)
            {
                foreach (var testCase in testCases)
                {
                    int count = testCasesInDb.Where(_ => _.TestLevelId == testCase.TestLevelId &&
                            _.Title == testCase.Title &&
                            _.Description == testCase.Description)
                        .ToList()
                        .Count();
                    if (0 == count)
                    {
                        uniqueTestCases.Add(testCase);
                    }
                }
            }
            else
            {
                uniqueTestCases = testCases.ToList();
            }

            _logger.LogInformation($"{nameof(uniqueTestCases)}.{nameof(uniqueTestCases.Count)} = {uniqueTestCases.Count}");
            if (0 < uniqueTestCases.Count())
            {
                var response = await _httpClient.PostAsJsonAsync("api/TestCase/Bulk", uniqueTestCases);
                if (!response.IsSuccessStatusCode)
                {
                    return new StatusCodeResult((int)response.StatusCode);
                }
            }
            return new OkResult();
        }

        public virtual async Task<IList<TestCaseDto>> ApplyTestCaseAsync(IList<TestCaseDto> testCases)
        {
            List<TestCaseDto>? testCasesInDb = await _httpClient.GetFromJsonAsync<List<TestCaseDto>>("api/TestCase");
            if (null != testCasesInDb)
            {
                foreach (var testCase in testCases)
                {
                    var regTestCase = testCasesInDb
                        .Where(_ => (_.TestLevelId == testCase.TestLevelId) &&
                            (_.Title == testCase.Title) && 
                            (_.Description == testCase.Description))
                        .FirstOrDefault();
                    testCase.Id = regTestCase!.Id;
                }
            }
            return testCases;
        }

        public virtual async Task<IActionResult> CreateTestResultAsync(ICollection<TestResultDto> testResults)
        {
            Task<HttpResponseMessage> responseTask = _httpClient.PostAsJsonAsync("api/TestResult/Bulk", testResults);
            responseTask.Wait();
            HttpResponseMessage response = responseTask.Result;
            if (response.IsSuccessStatusCode)
            {
                return new OkResult();
            }
            else
            {
                return new StatusCodeResult((int)response.StatusCode);
            }
        }
    }
}
