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

namespace TestManagement.APP.Pages.Project.Executions
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
        public SelectList EnvironmentSelectList = new SelectList(new List<TestManagement.APP.ViewModel.Environment.EnvironmentViewModel>(), "DisplayName", "DisplayName");

        /// <summary>
        /// Revision identifier (e.g. commit SHA) bound from the form.
        /// </summary>
        [BindProperty]
        public string Revision { get; set; } = string.Empty;

        /// <summary>
        /// Collection of available environments.
        /// </summary>
        public ICollection<EnvironmentViewModel> Environments { get; set; } = new List<EnvironmentViewModel>();

        /// <summary>
        /// Handles GET requests. Loads available environments and prepares the SelectList.
        /// </summary>
        public async Task OnGetAsync(long projectId)
        {
            _logger.LogInformation(
                "CreateModel::OnGetAsync() start. ProjectId: {ProjectId}",
                projectId);

            ICollection<EnvironmentViewModel>? environments =
                await _environmentService.GetEnvironmentsAsync();

            Environments = environments ?? new List<EnvironmentViewModel>();

            if (Environments.Count > 0)
            {
                EnvironmentSelectList = new SelectList(
                    Environments,
                    "DisplayName",
                    "DisplayName",
                    Environments.First());
            }
        }

        /// <summary>
        /// Handles POST requests to create a new test execution.
        /// Converts the posted datetime to UTC before sending to the API and redirects to Index on success.
        /// </summary>
        /// <returns>A redirect to the Index page on success, or the same page with model errors on failure.</returns>
        public async Task<IActionResult> OnPostAsync(long projectId)
        {
            _logger.LogInformation("CreateModel::OnPostAsync() start!");

            ICollection<EnvironmentViewModel>? environments =
                await _environmentService.GetEnvironmentsAsync();

            string environmentName = environments?
                .Where(environment => environment.Name == Environment)
                .Select(environment => environment.Name)
                .FirstOrDefault() ?? string.Empty;

            DateTime executedAtUtc;

            if (ExecutedAt.Kind == DateTimeKind.Utc)
            {
                executedAtUtc = ExecutedAt;
            }
            else if (ExecutedAt.Kind == DateTimeKind.Unspecified)
            {
                executedAtUtc =
                    DateTime.SpecifyKind(ExecutedAt, DateTimeKind.Local)
                        .ToUniversalTime();
            }
            else
            {
                executedAtUtc = ExecutedAt.ToUniversalTime();
            }

            var created = await _testExecutionService.CreateExecutionAsync(
                executedAtUtc,
                environmentName,
                Revision);

            if (created is not null && created.CreatedExecution is not null)
            {
                return RedirectToPage(
                    "/Project/Executions/Index",
                    new { projectId });
            }

            ModelState.AddModelError(
                string.Empty,
                "Failed to create test execution.");

            return Page();
        }
    }
}
