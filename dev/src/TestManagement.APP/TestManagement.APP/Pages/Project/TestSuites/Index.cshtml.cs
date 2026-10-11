using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TestManagement.APP.Services.Project;
using TestManagement.APP.ViewModel.Project;
using TestManagement.APP.ViewModel.TestSuite;

namespace TestManagement.APP.Pages.Project.TestSuites;

public class IndexModel : PageModel
{
    private readonly IProjectService _projectService;

    public IndexModel(IProjectService projectService)
    {
        _projectService = projectService;
    }

    public ProjectViewModel? Project { get; private set; }
    public ICollection<TestSuiteViewModel> TestSuites { get; private set; } = new List<TestSuiteViewModel>();

    public async Task<IActionResult> OnGetAsync(long projectId)
    {
        Project = await _projectService.GetProjectAsync(projectId);
        if (Project is null)
        {
            return NotFound();
        }

        TestSuites = await _projectService.GetProjectTestSuitesAsync(projectId);
        return Page();
    }
}
