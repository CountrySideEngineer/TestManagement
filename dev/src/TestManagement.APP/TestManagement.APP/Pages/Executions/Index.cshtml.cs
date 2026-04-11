using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TestManagement.APP.Services;

namespace TestManagement.APP.Pages.Executions
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        private readonly ITestExecutionService _testExecutionService;

        public IndexModel(
            ILogger<IndexModel> logger,
            ITestExecutionService testExecutionService
            ) : base()
        {
            _logger = logger;
            _testExecutionService = testExecutionService;
        }

        public void OnGet()
        {
            _logger.LogInformation("IndexModel::OnGet() start!");

        }
    }
}
