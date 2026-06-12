using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using TestManagement.APP.Models;

namespace TestManagement.APP.Services
{
    /// <summary>
    /// API client for uploading and managing test-related data.
    /// Provides methods to retrieve and create test levels, test runs, test cases, and test results.
    /// </summary>
    public class UploadApiClient
    {
        /// <summary>
        /// HTTP client used to make requests to the test API.
        /// </summary>
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Logger for recording diagnostic information and errors.
        /// </summary>
        private readonly ILogger<UploadApiClient> _logger;

        /// <summary>
        /// Constructs an instance of <see cref="UploadApiClient"/>.
        /// </summary>
        /// <param name="logger">Logger for this client.</param>
        /// <param name="httpClientFactory">Factory for creating configured HTTP clients.</param>
        public UploadApiClient(ILogger<UploadApiClient> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient("TestApiClient");
        }

        /// <summary>
        /// Retrieves the list of available test levels synchronously.
        /// </summary>
        /// <returns>A list of <see cref="TestLevelDto"/> objects, or an empty list if retrieval fails.</returns>
        public virtual List<TestLevelDto> GetTestLevel()
        {
            var testLevelsTask = _httpClient.GetFromJsonAsync<List<TestLevelDto>>("api/testlevels");
            testLevelsTask.Wait();
            var testLevels = testLevelsTask.Result;
            if (null == testLevels)
            {
                return new List<TestLevelDto>();
            }
            else
            {
                return testLevels;
            }
        }

        /// <summary>
        /// Retrieves the list of available test levels asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, containing a list of <see cref="TestLevelDto"/> objects, or an empty list if retrieval fails.</returns>
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

        /// <summary>
        /// Retrieves all test runs with their associated test results synchronously.
        /// </summary>
        /// <returns>A list of <see cref="TestRunDto"/> objects with associated test results, or an empty list if retrieval fails.</returns>
        public virtual List<TestRunDto> GetTestRun()
        {
            _logger.LogInformation("UploadApiClient::GetTestRun() start!");

            var testRunsTask = _httpClient.GetFromJsonAsync<List<TestRunDto>>("api/testrun/");
            testRunsTask.Wait();

            var testRuns = testRunsTask.Result;
            if (null == testRuns)
            {
                return new List<TestRunDto>();
            }
            var testResultsTask = _httpClient.GetFromJsonAsync<List<TestResultDto>>("api/testresult");
            testResultsTask.Wait();

            List<TestResultDto>? testResults = testResultsTask.Result;
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

        /// <summary>
        /// Retrieves all test runs with their associated test results asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, containing a list of <see cref="TestRunDto"/> objects with associated test results, or an empty list if retrieval fails.</returns>
        public virtual async Task<List<TestRunDto>> GetTestRunAsync()
        {
            _logger.LogInformation("UploadApiClient::GetTestRunAsync() start!");

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

        /// <summary>
        /// Creates a new test run via the API asynchronously.
        /// </summary>
        /// <param name="newRun">The test run to create.</param>
        /// <returns>The created <see cref="TestRunDto"/> on success.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="newRun"/> is null.</exception>
        /// <exception cref="Exception">Thrown if the API request fails or returns an invalid response.</exception>
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

        /// <summary>
        /// Creates new test cases in the database, filtering out duplicates asynchronously.
        /// </summary>
        /// <param name="testCases">The test cases to create.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the operation result (200 OK on success).</returns>
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

        /// <summary>
        /// Applies existing test cases from the database to the provided list,
        /// populating their IDs from the database records asynchronously.
        /// </summary>
        /// <param name="testCases">The test cases to apply database IDs to.</param>
        /// <returns>A list of <see cref="TestCaseDto"/> objects with database IDs populated.</returns>
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

        /// <summary>
        /// Creates multiple test results in the database asynchronously.
        /// </summary>
        /// <param name="testResults">Collection of test results to create.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the operation result (200 OK on success).</returns>
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
