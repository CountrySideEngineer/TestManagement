using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TestManagement.APP.Services.Project;
using TestManagement.APP.ViewModel.Project;
using TestManagement.APP.ViewModel.Tester;

namespace TestManagement.APP.Pages.Project.Testers;

public class IndexModel : PageModel
{
    private readonly IProjectService _projectService;

    public IndexModel(IProjectService projectService)
    {
        _projectService = projectService;
    }

    public ProjectViewModel? Project { get; private set; }
    public ICollection<TesterViewModel> Testers { get; private set; } = new List<TesterViewModel>();

    public async Task<IActionResult> OnGetAsync(long projectId)
    {
        Project = await _projectService.GetProjectAsync(projectId);
        if (Project is null)
        {
            return NotFound();
        }

        Testers = await _projectService.GetProjectTestersAsync(projectId);
        return Page();
    }
}
