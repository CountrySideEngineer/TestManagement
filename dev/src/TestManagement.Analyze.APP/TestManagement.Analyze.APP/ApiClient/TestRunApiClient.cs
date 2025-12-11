using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TestManagement.Analyze.APP.Model.DTO;

namespace TestManagement.Analyze.APP.ApiClient
{
    internal class TestRunApiClient
    {
        private readonly HttpClient _httpClient;

        public TestRunApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public TestRunDto? Add(TestRunDto testRun)
        {
            Task<HttpResponseMessage> responseTask = _httpClient.PostAsJsonAsync("TestRun", testRun);
            responseTask.Wait();
            HttpResponseMessage response = responseTask.Result;
            if (response.IsSuccessStatusCode)
            {
                Task<TestRunDto?> testRunTask =
                    response.Content.ReadFromJsonAsync<TestRunDto>();
                testRunTask.Wait();
                TestRunDto? newTestRun = testRunTask.Result;
                return newTestRun;
            }
            else
            {
                return null;
            }
        }
    }
}
