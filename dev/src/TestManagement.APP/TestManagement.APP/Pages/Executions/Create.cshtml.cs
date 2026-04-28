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
    /// <summary>
    /// Razor Page model used to create a new test execution record.
    /// Handles presenting available environments and accepting form input to create an execution.
    /// </summary>
    public class CreateModel : PageModel
    {
        /// <summary>
        /// Logger for diagnostic and informational messages emitted by this page model.
        /// </summary>
        private readonly ILogger<CreateModel> _logger;

        /// <summary>
        /// Service responsible for creating and retrieving test execution data.
        /// </summary>
        private readonly ITestExecutionService _testExecutionService;

        /// <summary>
        /// Service used to obtain available environments for selection in the UI.
        /// </summary>
        private readonly IEnvironmentService _environmentService;

        /// <summary>
        /// Constructs a new instance of <see cref="CreateModel"/>.
        /// </summary>
        /// <param name="logger">Logger provided by DI.</param>
        /// <param name="testExecutionService">Service to create executions.</param>
        /// <param name="environmentService">Service to retrieve environment list.</param>
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

        /// <summary>
        /// The execution timestamp bound from the form. Defaults to UTC now.
        /// Note: HTML datetime-local inputs bind to a DateTime with Kind == Unspecified.
        /// </summary>
        [BindProperty]
        public DateTime ExecutedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// The environment name selected in the form (bind property).
        /// </summary>
        [BindProperty]
        public string Environment { get; set; } = string.Empty;

        /// <summary>
        /// View model used to render and validate execution creation UI elements.
        /// </summary>
        public ExecutionCreateViewModel ExecutionCreateViewModel { get; set; } = new ExecutionCreateViewModel();

        /// <summary>
        /// SelectList of available environments used by the Razor view to populate a dropdown.
        /// </summary>
        public SelectList EnvironmentSelectList = new SelectList(new List<TestManagement.APP.ViewModel.Environment.EnvironmentModel>(), "DisplayName", "DisplayName");

        /// <summary>
        /// Revision identifier (e.g. commit SHA) bound from the form.
        /// </summary>
        [BindProperty]
        public string Revision { get; set; } = string.Empty;

        /// <summary>
        /// Handles GET requests. Loads available environments and prepares the SelectList.
        /// </summary>
        public async Task OnGetAsync()
        {
            _logger.LogInformation("CreateModel::OnGetAsync() start!");

            ICollection<EnvironmentModel>? envs = await _environmentService.GetEnvironmentsAsync();
            ICollection<EnvironmentModel> Environments = envs!;
            EnvironmentSelectList = new SelectList(Environments, "DisplayName", "DisplayName", Environments.ElementAt(0));
        }

        /// <summary>
        /// Handles POST requests to create a new test execution.
        /// Converts the posted datetime to UTC before sending to the API and redirects to Index on success.
        /// </summary>
        /// <returns>A redirect to the Index page on success, or the same page with model errors on failure.</returns>
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
