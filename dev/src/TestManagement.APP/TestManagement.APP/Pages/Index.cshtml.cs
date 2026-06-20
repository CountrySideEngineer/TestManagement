using Microsoft.AspNetCore.Mvc.RazorPages;
using TestManagement.APP.Dto.TestExecution.Get;
using TestManagement.APP.Models;
using TestManagement.APP.Services;
using TestManagement.APP.ViewModel.Dashboard;

namespace TestManagement.APP.Pages
{
    /// <summary>
    /// Page model for the home/dashboard page.
    /// Displays dashboard statistics and overview of test executions.
    /// </summary>
    public class IndexModel : PageModel
    {
        /// <summary>
        /// Logger instance for logging page-level events and errors.
        /// </summary>
        private readonly ILogger<IndexModel>? _logger;

        /// <summary>
        /// Service for retrieving dashboard data and statistics.
        /// </summary>
        private readonly IDashboardService? _dashboardService;

        /// <summary>
        /// Constructs an instance of <see cref="IndexModel"/>.
        /// </summary>
        /// <param name="logger">Logger for diagnostics.</param>
        /// <param name="dashboardService">Service to retrieve dashboard data.</param>
        public IndexModel(
            ILogger<IndexModel>? logger,
            IDashboardService? dashboardService
            ) : base()
        {
            _logger = logger;
            _dashboardService = dashboardService;
        }

        /// <summary>
        /// View model containing dashboard data to be displayed on the page.
        /// </summary>
        public DashboardViewModel? DashboardViewModel { get; set; }

        /// <summary>
        /// Handles GET requests for the home page.
        /// Retrieves dashboard data asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task OnGetAsync()
        {
            try
            {
                DashboardViewModel = await _dashboardService!.GetAsync();
            }
            catch (HttpRequestException ex)
            {
                _logger?.LogWarning(ex, "Failed to retrieve dashboard data due to a communication error.");
                ViewData["ErrorMessage"] = "データの取得に失敗しました(通信エラー)" +
                    "後ほど再実行してください。";

                DashboardViewModel = null;
            }
            catch (TaskCanceledException ex)
            {
                _logger?.LogWarning(ex, "The dashboard request timed out.");
                ViewData["ErrorMessage"] = "ダッシュボード取得がタイムアウトしました。";

                DashboardViewModel = null;
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "Failed to retrieve dashboard data due to an unexpected error.");
                ViewData["ErrorMessage"] = "予期せぬエラーが発生しました。管理者に問い合わせてください。";

                DashboardViewModel = null;
            }
        }
    }
}

/// <summary>
/// Data transfer object representing a summary of test execution statistics.
/// </summary>
/// <param name="ErrorNum">Number of tests that encountered errors.</param>
/// <param name="SkippedNum">Number of tests that were skipped.</param>
/// <param name="DisabledNum">Number of tests that were disabled.</param>
/// <param name="ExecutedNum">Total number of tests executed.</param>
public record SummaryDto(int ErrorNum, int SkippedNum, int DisabledNum, int ExecutedNum);

/// <summary>
/// Data transfer object representing a test execution record.
/// </summary>
/// <param name="ExecutedAt">The date and time when the test was executed.</param>
/// <param name="Result">The result of the test execution (e.g., "PASS", "FAIL").</param>
public record TestRecordDto(DateTime ExecutedAt, string Result);

/// <summary>
/// Data transfer object representing a trend of test requests over time.
/// </summary>
/// <param name="Time">The time period for the trend data.</param>
/// <param name="Count">The count of requests during the time period.</param>
public record RequestTrendDto(string Time, int Count);

/// <summary>
/// Data transfer object representing an error that occurred during test execution.
/// </summary>
/// <param name="Time">The time when the error occurred.</param>
/// <param name="Message">Description of the error message.</param>
public record ErrorDto(string Time, string Message);
