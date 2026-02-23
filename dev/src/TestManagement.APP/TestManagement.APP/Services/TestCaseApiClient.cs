using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.Win32;
using System.Diagnostics;
using System.Linq;
using TestManagement.APP.Models;

namespace TestManagement.APP.Services
{
    public class TestCaseApiClient
    {
        private readonly HttpClient _httpClient;

        public TestCaseApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("TestApiClient");
        }

        public async Task<List<Models.TestCaseDto>> GetTestCaseAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<TestCaseDto>>("api/TestCase");
            return response!;
        }

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

        public async Task<IActionResult> AddWithoutDuplicate(IList<TestCaseDto> testCases)
        {
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
                //Execute the test only if there are newly registered test case records.
                //If there are no new test cases to register, processing is not required.
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
