using Microsoft.AspNetCore.Mvc.RazorPages;
using TestManagement.APP.Services.Environment;
using TestManagement.APP.Services.TestExecution;
using TestManagement.APP.ViewModel.Environment;
using TestManagement.APP.ViewModel.Executions;

namespace TestManagement.APP.Pages.Executions
{
    /// <summary>
    /// Razor Page model for the Executions index page.
    /// Responsible for loading and exposing the list of test executions to the view.
    /// </summary>
    public class IndexModel : PageModel
    {
        /// <summary>
        /// Logger used to emit diagnostic messages from this page model.
        /// </summary>
        private readonly ILogger<IndexModel> _logger;

        /// <summary>
        /// Service that provides access to test execution data.
        /// </summary>
        private readonly ITestExecutionService _testExecutionService;

        /// <summary>
        /// List of executions exposed to the Razor page. May be empty.
        /// </summary>
        public IEnumerable<ExecutionViewModel>? Executions { get; set; } = new List<ExecutionViewModel>();

        /// <summary>
        /// Creates a new instance of the <see cref="IndexModel"/>.
        /// </summary>
        /// <param name="logger">Logger instance provided by DI.</param>
        /// <param name="testExecutionService">Service used to retrieve executions.</param>
        public IndexModel(
            ILogger<IndexModel> logger,
            ITestExecutionService testExecutionService
            ) : base()
        {
            _logger = logger;
            _testExecutionService = testExecutionService;
        }

        /// <summary>
        /// Handles GET requests for the Executions index page and loads executions.
        /// </summary>
        public async Task OnGetAsync()
        {
            _logger.LogInformation("IndexModel::OnGet() start!");

            Executions = await _testExecutionService.GetExecutionsAsync();
        }
    }
}
