namespace TestManagement.APP.ViewModel.Dashboard
{
    public class DashboardViewModel
    {
        public LatestExecutionSummaryViewModel? LatestExecutionSummary { get; init; }

        public IEnumerable<RecentExecutionViewModel>? RecentExecutions { get; init; }
    }
}
