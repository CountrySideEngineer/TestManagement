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

        public SelectList EnvironmentSelectList = new SelectList(new List<TestManagement.APP.ViewModel.Environment.EnvironmentModel>(), "DisplayName", "DisplayName");

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
            _logger.LogInformation("CreateModel::OnPostAsync() start!");

            ICollection<EnvironmentModel>? environments = await _environmentService.GetEnvironmentsAsync();
            string environmentName = environments?
                .Where(_ => _.Name == Environment)
                .Select(_ => _.Name)
                .FirstOrDefault() ?? string.Empty;

            // HTML `input type="datetime-local"` posts a value without timezone information.
            // Model binding produces a DateTime with Kind == Unspecified in that case.
            // Treat unspecified as local time and convert to UTC before sending to the API.
            DateTime executedAtUtc;
            if (ExecutedAt.Kind == DateTimeKind.Utc)
            {
                executedAtUtc = ExecutedAt;
            }
            else if (ExecutedAt.Kind == DateTimeKind.Unspecified)
            {
                // Assume the posted value represents local time on the client/server and convert to UTC.
                executedAtUtc = DateTime.SpecifyKind(ExecutedAt, DateTimeKind.Local).ToUniversalTime();
            }
            else
            {
                // Local -> UTC
                executedAtUtc = ExecutedAt.ToUniversalTime();
            }

            var created = await _testExecutionService.CreateExecutionAsync(executedAtUtc, environmentName, Revision);

            if (created is not null && created.CreatedExecution is not null)
            {
                // Redirect to Index or to a details page; currently redirecting to Index.
                return RedirectToPage("Index");
            }

            ModelState.AddModelError(string.Empty, "Failed to create test execution.");

            return Page();
        }
    }
}
