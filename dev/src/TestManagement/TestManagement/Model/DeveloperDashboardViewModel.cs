namespace TestManagement.Model
{
    public class DeveloperDashboardViewModel
    {
        public ProjectSummary Summary { get; set; } = new ProjectSummary();
        public List<TestRunSummary> RecentRuns { get; set; } = new();
        public TestResultStatistics Statistics { get; set; } = new TestResultStatistics();
        public List<TestResultTrendPoint> Trend { get; set; } = new();
    }
}
