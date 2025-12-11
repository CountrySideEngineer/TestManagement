using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TestManagement.Analyze.APP.Model.DTO;

namespace TestManagement.Analyze.APP.ApiClient
{
    internal class TestCaseApiClient
    {
        private readonly HttpClient _httpClient;

        public TestCaseApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public ICollection<TestCaseDto>? GetAll()
        {
            Task<HttpResponseMessage> responseTask = _httpClient.GetAsync("TestCase");
            responseTask.Wait();
            HttpResponseMessage response = responseTask.Result;
            if (response.IsSuccessStatusCode)
            {
                Task<ICollection<TestCaseDto>?> testCasesTask = 
                    response.Content.ReadFromJsonAsync<ICollection<TestCaseDto>>();
                testCasesTask.Wait();
                ICollection<TestCaseDto>? testCases = testCasesTask.Result;
                return testCases;
            }
            else
            {
                return null;    
            }
        }

        public bool Add(ICollection<TestCaseDto> testCases)
        {
            Task<HttpResponseMessage> task = _httpClient.PostAsJsonAsync("TestCase/Bulk", testCases);
            task.Wait();
            HttpResponseMessage response = task.Result;
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
