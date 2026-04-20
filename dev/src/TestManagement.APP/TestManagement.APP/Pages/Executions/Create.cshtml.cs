using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Json;
using TestManagement.APP.Dto.TestExecution.Get;

namespace TestManagement.APP.Pages.Executions
{
    public class CreateModel : PageModel
    {
        private readonly ILogger<CreateModel> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public CreateModel(ILogger<CreateModel> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public DateTime ExecutedAt { get; set; } = DateTime.UtcNow;

        [BindProperty]
        public string Environment { get; set; } = string.Empty;

        [BindProperty]
        public string Revision { get; set; } = string.Empty;

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var client = _httpClientFactory.CreateClient("TestApiClient");

            var payload = new
            {
                executedAt = ExecutedAt,
                environment = Environment,
                revision = Revision
            };

            var response = await client.PostAsJsonAsync("api/testexecution", payload);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("./Index");
            }

            _logger.LogError("Failed to create test execution. StatusCode={StatusCode}", response.StatusCode);
            ModelState.AddModelError(string.Empty, "Failed to create test execution.");
            return Page();
        }
    }
}
