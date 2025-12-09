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
        private string _baseUrl = string.Empty;

        public TestCaseApiClient(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        public ICollection<TestCaseDto>? GetAll()
        {
            string apiUrl = @$"{_baseUrl}/TestCase";
            var httpClient = new HttpClient();
            Task<HttpResponseMessage> task = httpClient.GetAsync(apiUrl);
            task.Wait();
            HttpResponseMessage response = task.Result;
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
            string apiUrl = @$"{_baseUrl}/TestCase/Bulk";
            var httpClient = new HttpClient();
            Task<HttpResponseMessage> task = httpClient.PostAsJsonAsync(apiUrl,testCases);
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
