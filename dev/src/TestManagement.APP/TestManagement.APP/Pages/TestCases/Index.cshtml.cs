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
    /// <summary>
    /// Razor Page model for the TestCases index page.
    /// Responsible for retrieving and exposing the list of test cases to the view.
    /// </summary>
    public class IndexModel : PageModel
    {
        /// <summary>
        /// Api client used to fetch test case data from the backend.
        /// </summary>
        private readonly TestCaseApiClient _apiClient;

        /// <summary>
        /// The collection of test cases displayed on the page.
        /// </summary>
        public IList<TestCaseDto> TestCases { get; set; } = new List<TestCaseDto>();

        /// <summary>
        /// Constructs the page model with the required API client.
        /// </summary>
        /// <param name="apiClient">Client used to query test cases.</param>
        public IndexModel(TestCaseApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        /// <summary>
        /// Handles GET requests and loads test cases from the API.
        /// Exceptions are swallowed to avoid UI errors when loading fails.
        /// </summary>
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