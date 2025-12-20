using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TestManagement.Analyze.APP.Model.DTO;

namespace TestManagement.Analyze.APP.ApiClient
{
    internal class TestResultApiClient
    {
        private readonly HttpClient _httpClient;

        public TestResultApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public ICollection<TestResultDto>? Add(ICollection<TestResultDto> results)
        {
            Task<HttpResponseMessage> responseTask = _httpClient.PostAsJsonAsync("TestResult/Bulk", results);
            responseTask.Wait();
            HttpResponseMessage responseMessage = responseTask.Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                Task<ICollection<TestResultDto>?> testResultTask = 
                    responseMessage.Content.ReadFromJsonAsync<ICollection<TestResultDto>>();
                testResultTask.Wait();
                ICollection<TestResultDto>? newTestResults = testResultTask.Result;
                return newTestResults;
            }
            else
            {
                return null;
            }
        }
    }
}
