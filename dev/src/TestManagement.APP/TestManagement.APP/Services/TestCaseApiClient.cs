using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.Win32;
using System.Diagnostics;
using System.Linq;
using TestManagement.APP.Models;

namespace TestManagement.APP.Services
{
    /// <summary>
    /// API client responsible for test case related HTTP operations.
    /// Provides methods to query, create (bulk) and update test case records.
    /// </summary>
    public class TestCaseApiClient
    {
        /// <summary>
        /// HttpClient configured for the Test API (named client "TestApiClient").
        /// </summary>
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Creates a new <see cref="TestCaseApiClient"/> using the provided <see cref="IHttpClientFactory"/>.
        /// </summary>
        /// <param name="httpClientFactory">Factory used to create the configured HttpClient.</param>
        public TestCaseApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("TestApiClient");
        }

        /// <summary>
        /// Retrieves all test cases from the backend API.
        /// </summary>
        /// <returns>List of <see cref="Models.TestCaseDto"/>. May throw on HTTP failures.</returns>
        public async Task<List<Models.TestCaseDto>> GetTestCaseAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<TestCaseDto>>("api/TestCase");
            return response!;
        }

        /// <summary>
        /// Posts a collection of test cases to the API in bulk.
        /// </summary>
        /// <param name="testCases">Collection of test cases to add.</param>
        /// <returns>An <see cref="IActionResult"/> indicating success (200) or the HTTP status code on failure.</returns>
        public async Task<IActionResult> Add(IList<TestCaseDto> testCases)
        {
            var response = await _httpClient.PostAsJsonAsync("api/TestCase/Bulk", testCases);
            if (response.IsSuccessStatusCode)
            {
                return new OkResult();
            }
            else
            {
                return new StatusCodeResult((int)response.StatusCode);
            }
        }

        /// <summary>
        /// Updates a single test case by id.
        /// </summary>
        /// <param name="testCase">Test case to update. The <c>Id</c> property is used in the request URL.</param>
        /// <returns>An <see cref="IActionResult"/> indicating success (200) or the HTTP status code on failure.</returns>
        public async Task<IActionResult> Update(TestCaseDto testCase)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/TestCase/{testCase.Id}", testCase);
            if (response.IsSuccessStatusCode)
            {
                return new OkResult();
            }
            else
            {
                return new StatusCodeResult((int)response.StatusCode);
            }
        }

        /// <summary>
        /// Adds test cases while attempting to avoid duplicates.
        /// This method fetches currently registered test cases, concatenates with the provided list,
        /// and derives a list of unique entries based on (<c>TestLevelId</c>, <c>Title</c>, <c>Description</c>).
        /// If no unique/new items are found the method returns success without calling the API.
        /// </summary>
        /// <param name="testCases">List of test cases to add.</param>
        /// <returns>An <see cref="IActionResult"/> indicating success or the HTTP status code on failure.</returns>
        public async Task<IActionResult> AddWithoutDuplicate(IList<TestCaseDto> testCases)
        {
            // Note: synchronous .Result is used here to get the registered items. This mirrors the original behavior.
            List<TestCaseDto> registered = GetTestCaseAsync().Result;
            registered.AddRange(testCases);
            IEnumerable<TestCaseDto> query = testCases.Concat(registered);
            var uniqueList = query.GroupBy(_ => (_.TestLevelId, _.Title, _.Description))
                .Where(_ => _.Count() == 1)
                .SelectMany(_ => _)
                .ToList();

            Debug.WriteLine($"{nameof(uniqueList)}.{nameof(uniqueList.Count)} = {uniqueList.Count}");
            if (0 == uniqueList.Count)
            {
                // Execute the test only if there are newly registered test case records.
                // If there are no new test cases to register, processing is not required.
                return new OkResult();
            }

            var response = await _httpClient.PostAsJsonAsync("api/TestCase/Bulk", testCases);
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
