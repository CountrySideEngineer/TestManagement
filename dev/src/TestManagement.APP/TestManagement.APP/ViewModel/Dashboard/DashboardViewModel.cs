namespace TestManagement.APP.ViewModel.Dashboard
{
    public class DashboardViewModel
    {
        /// <summary>
        /// Summary information for the most recent execution, or null if none is available.
        /// </summary>
        public LatestExecutionSummaryViewModel? LatestExecutionSummary { get; init; }

        /// <summary>
        /// A collection of recent execution records ordered by execution time (most recent first).
        /// </summary>
        public IEnumerable<RecentExecutionViewModel>? RecentExecutions { get; init; }
    }
}
