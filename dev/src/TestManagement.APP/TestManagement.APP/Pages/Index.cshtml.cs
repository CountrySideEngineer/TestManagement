using Microsoft.AspNetCore.Mvc.RazorPages;
using TestManagement.APP.Services;
using TestManagement.APP.Services.Project;
using TestManagement.APP.ViewModel.Project;

namespace TestManagement.APP.Pages
{
    /// <summary>
    /// Page model for the home page.
    /// Displays the list of projects.
    /// </summary>
    public class IndexModel : PageModel
    {
        /// <summary>
        /// Logger instance for logging page-level events and errors.
        /// </summary>
        private readonly ILogger<IndexModel>? _logger;

        /// <summary>
        /// Service for retrieving projects.
        /// </summary>
        private readonly IProjectService _projectService;

        /// <summary>
        /// Constructs an instance of <see cref="IndexModel"/>.
        /// </summary>
        /// <param name="logger">Logger for diagnostics.</param>
        /// <param name="projectService">Service to retrieve projects.</param>
        public IndexModel(
            ILogger<IndexModel>? logger,
            IProjectService projectService
            ) : base()
        {
            _logger = logger;
            _projectService = projectService;
        }

        /// <summary>
        /// Projects displayed on the home page.
        /// </summary>
        public ICollection<ProjectViewModel> Projects { get; private set; } = new List<ProjectViewModel>();

        /// <summary>
        /// Handles GET requests for the home page.
        /// Retrieves projects asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task OnGetAsync()
        {
            try
            {
                Projects = await _projectService.GetProjectsAsync();
            }
            catch (HttpRequestException ex)
            {
                _logger?.LogWarning(ex, "Failed to retrieve projects due to a communication error.");
                ViewData["ErrorMessage"] = "データの取得に失敗しました(通信エラー)" +
                    "後ほど再実行してください。";

                Projects = new List<ProjectViewModel>();
            }
            catch (TaskCanceledException ex)
            {
                _logger?.LogWarning(ex, "The project request timed out.");
                ViewData["ErrorMessage"] = "プロジェクト取得がタイムアウトしました。";

                Projects = new List<ProjectViewModel>();
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "Failed to retrieve projects due to an unexpected error.");
                ViewData["ErrorMessage"] = "予期せぬエラーが発生しました。管理者に問い合わせてください。";

                Projects = new List<ProjectViewModel>();
            }
        }
    }
}
