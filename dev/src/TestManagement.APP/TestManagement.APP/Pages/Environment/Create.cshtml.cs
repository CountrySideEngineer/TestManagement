using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TestManagement.APP.ApiClients.Environment;
using TestManagement.APP.Dto.Environment.Create;
using TestManagement.APP.Dto.Environment.Post;
using TestManagement.APP.Services.Environment;

namespace TestManagement.APP.Pages.Environments
{
    public class CreateModel : PageModel
    {
        private readonly ILogger<CreateModel>? _logger;
        private readonly IEnvironmentService? _environmentService;

        public CreateModel(ILogger<CreateModel>? logger, IEnvironmentService? environmentService)
        {
            _logger = logger;
            _environmentService = environmentService;
        }

        [BindProperty]
        public PostEnvironmentRequest Environment { get; set; } = new PostEnvironmentRequest();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (_environmentService is null)
            {
                _logger?.LogError("IEnvironmentApiClient is not available via DI.");
                ModelState.AddModelError(string.Empty, "サービスが利用できません。");
                return Page();
            }

            try
            {
                var request = new CreateEnvironmentRequest()
                {
                    Name = Environment.Name,
                    Os = Environment.Os,
                    RunTime = Environment.RunTime
                };
                await _environmentService.CreateEnvironmentAsync(request);
                TempData["SuccessMessage"] = "環境を登録しました。";
                return RedirectToPage("/Index");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "環境登録に失敗しました。");
                ModelState.AddModelError(string.Empty, $"環境登録に失敗しました: {ex.Message}");
                return Page();
            }
        }
    }
}