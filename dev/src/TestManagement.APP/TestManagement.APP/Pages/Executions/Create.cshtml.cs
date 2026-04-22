using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http;
using System.Net.Http.Json;
using TestManagement.APP.Dto.TestExecution.Get;
using TestManagement.APP.Services;
using TestManagement.APP.ViewModel.Executions;

namespace TestManagement.APP.Pages.Executions
{
    public class CreateModel : PageModel
    {
        private readonly ILogger<CreateModel> _logger;

        private readonly ITestExecutionService _testExecutionService;
        private readonly IHttpClientFactory _httpClientFactory;

        public CreateModel(
            ILogger<CreateModel> logger,
            ITestExecutionService testExecutionService,
            IHttpClientFactory httpClientFactory
            )
        {
            _logger = logger;
            _testExecutionService = testExecutionService;
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public DateTime ExecutedAt { get; set; } = DateTime.UtcNow;

        [BindProperty]
        public string Environment { get; set; } = string.Empty;

        public ExecutionCreateViewModel ExecutionCreateViewModel { get; set; } = new ExecutionCreateViewModel();

        public SelectList EnvironmentSelectList = new SelectList(new List<string>(), "EnvironmentId", "DisplayName");

        [BindProperty]
        public string Revision { get; set; } = string.Empty;

        public async Task OnGetAsync()
        {
            _logger.LogInformation("CreateModel::OnGetAsync() start!");

            ExecutionCreateViewModel = await _testExecutionService.GetExecutionCreateViewModelsAsync();
            EnvironmentSelectList = new SelectList(
                ExecutionCreateViewModel.Environments, "DisplayName", "DisplayName",
                ExecutionCreateViewModel.Environments.ElementAt(0).DisplayName
                );
        }

        public async Task<IActionResult> OnPostAsync()
        {
            return Page();
        }
    }
}
