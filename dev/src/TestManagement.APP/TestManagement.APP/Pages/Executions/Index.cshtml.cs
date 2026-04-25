using Microsoft.AspNetCore.Mvc.RazorPages;
using TestManagement.APP.Services.Environment;
using TestManagement.APP.Services.TestExecution;
using TestManagement.APP.ViewModel.Environment;
using TestManagement.APP.ViewModel.Executions;

namespace TestManagement.APP.Pages.Executions
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        private readonly ITestExecutionService _testExecutionService;

        public IEnumerable<ExecutionIndexViewModel>? Executions { get; set; } = new List<ExecutionIndexViewModel>();

        public IndexModel(
            ILogger<IndexModel> logger,
            ITestExecutionService testExecutionService
            ) : base()
        {
            _logger = logger;
            _testExecutionService = testExecutionService;
        }

        public async Task OnGetAsync()
        {
            _logger.LogInformation("IndexModel::OnGet() start!");

            Executions = await _testExecutionService.GetExecutionsAsync();
        }
    }
}
