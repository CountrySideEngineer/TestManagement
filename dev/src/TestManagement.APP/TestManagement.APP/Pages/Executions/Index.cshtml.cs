using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TestManagement.APP.Services;
using TestManagement.APP.ViewModel.Executions;

namespace TestManagement.APP.Pages.Executions
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        private readonly ITestExecutionService _testExecutionService;

        public IEnumerable<ExecutionIndexViewModel>? Executions { get; set; }

        public IndexModel(
            ILogger<IndexModel> logger,
            ITestExecutionService testExecutionService
            ) : base()
        {
            _logger = logger;
            _testExecutionService = testExecutionService;
        }

        public async void OnGetAsync()
        {
            _logger.LogInformation("IndexModel::OnGet() start!");

            Executions = await _testExecutionService.GetExecutionsAsync();
        }
    }
}
