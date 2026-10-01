using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TestManagement.APP.Services.Project;
using TestManagement.APP.ViewModel.Project;

namespace TestManagement.APP.Pages.Project;

/// <summary>
/// Page model for displaying a project's details.
/// </summary>
public class IndexModel : PageModel
{
    /// <summary>
    /// Logger used to record project page diagnostics.
    /// </summary>
    private readonly ILogger<IndexModel> _logger;

    /// <summary>
    /// Service used to retrieve project information.
    /// </summary>
    private readonly IProjectService _projectService;

    /// <summary>
    /// Initializes a new instance of the <see cref="IndexModel"/> class.
    /// </summary>
    /// <param name="logger">Logger used for project page diagnostics.</param>
    /// <param name="projectService">Service used to retrieve project information.</param>
    public IndexModel(
        ILogger<IndexModel> logger,
        IProjectService projectService)
    {
        _logger = logger;
        _projectService = projectService;
    }

    /// <summary>
    /// Gets the project displayed on the page.
    /// </summary>
    public ProjectViewModel? Project { get; private set; }

    /// <summary>
    /// Handles GET requests and loads the requested project.
    /// </summary>
    /// <param name="id">The identifier of the project to retrieve.</param>
    /// <returns>The project page, a not-found result, or a bad-gateway result when the API cannot be reached.</returns>
    public async Task<IActionResult> OnGetAsync(long? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        try
        {
            Project = await _projectService.GetProjectAsync(id.Value);
            return Project is null ? NotFound() : Page();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "プロジェクトの取得に失敗しました。Id: {Id}", id.Value);
            return StatusCode(StatusCodes.Status502BadGateway);
        }
    }
}
