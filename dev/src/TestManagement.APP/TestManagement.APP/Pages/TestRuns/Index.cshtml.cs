using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TestManagement.APP.Models;
using TestManagement.APP.Services;

namespace TestManagement.APP.Pages.TestRuns
{
    /// <summary>
    /// Razor Page model for the TestRuns index page.
    /// Exposes the list of test runs retrieved from the API to the Razor view.
    /// </summary>
    public class IndexModel : PageModel
    {
        /// <summary>
        /// API client used to retrieve test run information.
        /// </summary>
        private readonly TestRunApiClient _apiClient;

        /// <summary>
        /// Constructs the page model with a TestRunApiClient.
        /// </summary>
        /// <param name="apiClient">Client used to fetch test runs.</param>
        public IndexModel(TestRunApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        /// <summary>
        /// Collection of test runs shown on the page.
        /// </summary>
        public IList<TestRunDto> TestRuns { get; set; } = new List<TestRunDto>();

        /// <summary>
        /// Handles GET requests and loads test runs from the API.
        /// </summary>
        public async Task OnGetAsync()
        {
            TestRuns = await _apiClient.GetTestRunsAsync();
        }
    }
}
