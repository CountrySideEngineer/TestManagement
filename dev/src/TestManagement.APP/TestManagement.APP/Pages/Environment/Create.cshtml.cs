using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TestManagement.APP.ApiClients.Environment;
using TestManagement.APP.Dto.Environment.Post;

namespace TestManagement.APP.Pages.Environments
{
    public class CreateModel : PageModel
    {
        private readonly ILogger<CreateModel>? _logger;
        private readonly IEnvironmentApiClient? _environmentApiClient;

        public CreateModel(ILogger<CreateModel>? logger, IEnvironmentApiClient? environmentApiClient)
        {
            _logger = logger;
            _environmentApiClient = environmentApiClient;
        }

        [BindProperty]
        public PostEnvironmentRequest Environment { get; set; } = new PostEnvironmentRequest();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            return Page();
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

            //if (_environmentApiClient is null)
            //{
            //    _logger?.LogError("IEnvironmentApiClient is not available via DI.");
            //    ModelState.AddModelError(string.Empty, "ƒT[ƒrƒX‚ª—˜—p‚Å‚«‚Ü‚¹‚ñB");
            //    return Page();
            //}

            //try
            //{
            //    await _environmentApiClient.CreateEnvironmentAsync(Environment);
            //    TempData["SuccessMessage"] = "ŠÂ‹«‚ğ“o˜^‚µ‚Ü‚µ‚½B";
            //    return RedirectToPage("/Index");
            //}
            //catch (Exception ex)
            //{
            //    _logger?.LogError(ex, "ŠÂ‹«“o˜^‚É¸”s‚µ‚Ü‚µ‚½B");
            //    ModelState.AddModelError(string.Empty, $"ŠÂ‹«“o˜^‚É¸”s‚µ‚Ü‚µ‚½: {ex.Message}");
            //    return Page();
            //}
        }
    }
}