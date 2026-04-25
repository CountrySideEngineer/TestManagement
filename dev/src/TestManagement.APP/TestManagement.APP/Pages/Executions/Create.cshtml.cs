using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http;
using System.Net.Http.Json;
using TestManagement.APP.Dto.TestExecution.Get;
using TestManagement.APP.Services.Environment;
using TestManagement.APP.Services.TestExecution;
using TestManagement.APP.ViewModel.Environment;
using TestManagement.APP.ViewModel.Executions;

namespace TestManagement.APP.Pages.Executions
{
    public class CreateModel : PageModel
    {
        private readonly ILogger<CreateModel> _logger;

        private readonly ITestExecutionService _testExecutionService;

        private readonly IEnvironmentService _environmentService;

        public CreateModel(
            ILogger<CreateModel> logger,
            ITestExecutionService testExecutionService,
            IEnvironmentService environmentService
            )
        {
            _logger = logger;
            _testExecutionService = testExecutionService;
            _environmentService = environmentService;
        }

        [BindProperty]
        public DateTime ExecutedAt { get; set; } = DateTime.UtcNow;

        [BindProperty]
        public string Environment { get; set; } = string.Empty;

        public ICollection<EnvironmentModel> Environments { get; set; } = new List<EnvironmentModel>();

        public ExecutionCreateViewModel ExecutionCreateViewModel { get; set; } = new ExecutionCreateViewModel();

        public SelectList EnvironmentSelectList = new SelectList(new List<string>(), "EnvironmentId", "DisplayName");

        [BindProperty]
        public string Revision { get; set; } = string.Empty;

        public async Task OnGetAsync()
        {
            _logger.LogInformation("CreateModel::OnGetAsync() start!");

            ICollection<EnvironmentModel>? envs = await _environmentService.GetEnvironmentsAsync();
            Environments = envs!;

            EnvironmentSelectList = new SelectList(Environments, "DisplayName", "DisplayName", Environments.ElementAt(0));
        }

        public async Task<IActionResult> OnPostAsync()
        {
            return Page();
        }
    }
}
