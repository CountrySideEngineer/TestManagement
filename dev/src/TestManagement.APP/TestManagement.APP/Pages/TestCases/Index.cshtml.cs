using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TestManagement.APP.Models;
using TestManagement.APP.Services;

namespace TestManagement.APP.Pages.TestCases
{
    public class IndexModel : PageModel
    {
        private readonly TestCaseApiClient _apiClient;

        public IList<TestCaseDto> TestCases { get; set; } = new List<TestCaseDto>();

        public IndexModel(TestCaseApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task OnGetAsync()
        {
            try
            {
                TestCases = await _apiClient.GetTestCaseAsync();
            }
            catch (Exception)
            {
            }
        }
    }
}