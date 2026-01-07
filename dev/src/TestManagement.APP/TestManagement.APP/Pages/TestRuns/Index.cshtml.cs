using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TestManagement.APP.Models;
using TestManagement.APP.Services;

namespace TestManagement.APP.Pages.TestRuns
{
    public class IndexModel : PageModel
    {
        private readonly TestRunApiClient _apiClient;

        public IndexModel(TestRunApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public IList<TestRunDto> TestRuns { get; set; } = new List<TestRunDto>();

        public async Task OnGetAsync()
        {
            TestRuns = await _apiClient.GetTestRunsAsync();
        }
    }
}
