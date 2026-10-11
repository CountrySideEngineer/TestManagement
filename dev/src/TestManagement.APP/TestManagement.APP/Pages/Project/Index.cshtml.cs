using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using TestManagement.APP.Services;
using TestManagement.APP.Services.Project;
using TestManagement.APP.Services.TestExecution;
using TestManagement.APP.ViewModel.Project;
using TestManagement.APP.ViewModel.Executions;

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
    /// Service used to retrieve test execution information.
    /// </summary>
    private readonly ITestExecutionService _testExecutionService;

    /// <summary>
    /// Initializes a new instance of the <see cref="IndexModel"/> class.
    /// </summary>
    /// <param name="logger">Logger used for project page diagnostics.</param>
    /// <param name="projectService">Service used to retrieve project information.</param>
    /// <param name="testExecutionService">Service used to retrieve test execution information.</param>
    public IndexModel(
        ILogger<IndexModel> logger,
        IProjectService projectService,
        ITestExecutionService testExecutionService)
    {
        _logger = logger;
        _projectService = projectService;
        _testExecutionService = testExecutionService;
    }

    /// <summary>
    /// Gets the project displayed on the page.
    /// </summary>
    public ProjectViewModel? Project { get; private set; }

    /// <summary>
    /// Gets the most recent test executions displayed in the dashboard.
    /// </summary>
    public ICollection<ExecutionViewModel> RecentExecutions { get; private set; } = new List<ExecutionViewModel>();

    /// <summary>
    /// Gets the total number of test executions.
    /// </summary>
    public int ExecutionCount { get; private set; }

    /// <summary>
    /// Gets the total number of test cases across all result categories.
    /// </summary>
    public long TotalTestCases { get; private set; }

    /// <summary>
    /// Gets the total number of passed test cases.
    /// </summary>
    public long PassedTestCases { get; private set; }

    /// <summary>
    /// Gets the total number of failed test cases.
    /// </summary>
    public long FailedTestCases { get; private set; }

    /// <summary>
    /// Gets the total number of skipped test cases.
    /// </summary>
    public long SkippedTestCases { get; private set; }

    /// <summary>
    /// Gets the total number of excluded test cases.
    /// </summary>
    public long ExcludedTestCases { get; private set; }

    /// <summary>
    /// Gets the overall success rate as a percentage.
    /// </summary>
    public double SuccessRate { get; private set; }

    /// <summary>
    /// Gets the JSON-serialized labels used by the execution trend chart.
    /// </summary>
    public string ChartLabelsJson { get; private set; } = "[]";

    /// <summary>
    /// Gets the JSON-serialized passed test counts used by the execution trend chart.
    /// </summary>
    public string PassedTrendJson { get; private set; } = "[]";

    /// <summary>
    /// Gets the JSON-serialized failed test counts used by the execution trend chart.
    /// </summary>
    public string FailedTrendJson { get; private set; } = "[]";

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

            // Get the number of test execution.
            var executions = await _testExecutionService.GetExecutionsAsync() ?? new List<ExecutionViewModel>();

            // Retrieve only 10 test cases to display the 10 most recent test cases.
            RecentExecutions = executions.OrderByDescending(execution => execution.ExecutedAt).Take(10).ToList();

            // Retrieve the number of test cases, success rate, and failure count for the test.
            ExecutionCount = executions.Count;
            PassedTestCases = executions.Sum(execution => execution.PassedNum);
            FailedTestCases = executions.Sum(execution => execution.ErrorNum);
            SkippedTestCases = executions.Sum(execution => execution.SkippedNum);
            ExcludedTestCases = executions.Sum(execution => execution.ExcludedNum);
            TotalTestCases = PassedTestCases + FailedTestCases + SkippedTestCases + ExcludedTestCases;
            SuccessRate = PassedTestCases + FailedTestCases == 0
                ? 0
                : PassedTestCases * 100d / (PassedTestCases + FailedTestCases);

            var trend = executions.OrderBy(execution => execution.ExecutedAt).TakeLast(14).ToList();
            ChartLabelsJson = JsonSerializer.Serialize(trend.Select(execution => execution.ExecutedAt.ToLocalTime().ToString("MM/dd")));
            PassedTrendJson = JsonSerializer.Serialize(trend.Select(execution => execution.PassedNum));
            FailedTrendJson = JsonSerializer.Serialize(trend.Select(execution => execution.ErrorNum));

            return Project is null ? NotFound() : Page();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "プロジェクトの取得に失敗しました。Id: {Id}", id.Value);
            return StatusCode(StatusCodes.Status502BadGateway);
        }
    }
}
