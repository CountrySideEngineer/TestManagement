using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TestManagement.APP.Models;
using TestManagement.APP.Services;

namespace TestManagement.APP.Pages
{
    public class IndexModel : PageModel
    {
        private readonly TestLevelApiClient _apiClient;

        public IndexModel(TestLevelApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public List<TestLevelDto> TestLevels { get; set; } = new();

        //private readonly ILogger<IndexModel> _logger;

        //public IndexModel(ILogger<IndexModel> logger)
        //{
        //    _logger = logger;
        //}

        //public void OnGet()
        //{

        //}

        public async Task OnGetAsync()
        {
            TestLevels = await _apiClient.GetTestLevelsAsync();
        }
    }
}
