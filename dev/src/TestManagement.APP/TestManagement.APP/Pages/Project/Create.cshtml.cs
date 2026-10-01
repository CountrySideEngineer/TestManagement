using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TestManagement.APP.Dto.Project.Create;
using TestManagement.APP.Services.Project;

namespace TestManagement.APP.Pages.Project;

public class CreateModel : PageModel
{
    private readonly ILogger<CreateModel> _logger;
    private readonly IProjectService _projectService;

    public CreateModel(
        ILogger<CreateModel> logger,
        IProjectService projectService)
    {
        _logger = logger;
        _projectService = projectService;
    }

    [BindProperty]
    public CreateProjectRequest Project { get; set; } = new();

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            await _projectService.CreateProjectAsync(Project);
            TempData["SuccessMessage"] = "プロジェクトを追加しました。";
            return RedirectToPage("/Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "プロジェクトの追加に失敗しました。");
            ModelState.AddModelError(string.Empty, "プロジェクトの追加に失敗しました。");
            return Page();
        }
    }
}
