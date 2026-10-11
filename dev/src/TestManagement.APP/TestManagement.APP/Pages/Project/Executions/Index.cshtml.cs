using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TestManagement.APP.Services.Project;
using TestManagement.APP.Services.TestExecution;
using TestManagement.APP.ViewModel.Executions;
using TestManagement.APP.ViewModel.Project;

namespace TestManagement.APP.Pages.Project.Executions;

public class IndexModel : PageModel
{
    private readonly IProjectService _projectService;
    private readonly ITestExecutionService _testExecutionService;

    public IndexModel(IProjectService projectService, ITestExecutionService testExecutionService)
    {
        _projectService = projectService;
        _testExecutionService = testExecutionService;
    }

    public ProjectViewModel? Project { get; private set; }
    public ICollection<ExecutionViewModel> Executions { get; private set; } = new List<ExecutionViewModel>();

    public async Task<IActionResult> OnGetAsync(long projectId)
    {
        Project = await _projectService.GetProjectAsync(projectId);
        if (Project is null)
        {
            return NotFound();
        }

        Executions = (await _testExecutionService.GetExecutionsAsync() ?? new List<ExecutionViewModel>())
            .OrderByDescending(execution => execution.ExecutedAt)
            .ToList();

        return Page();
    }
}
