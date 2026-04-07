using Microsoft.AspNetCore.Hosting.Server;

namespace TestManagement.APP.ViewModel.Dashboard
{
    public class LatestExecutionSummaryViewModel
    {
        public string Revision { get; init; } = string.Empty;

        public string EnvironmentName { get; init; } = string.Empty;

        public long ErrorNum { get; init; } = 0;

        public long SkippedNum { get; init; } = 0;

        public long ExcludedNum { get; init; } = 0;

        public long ExecutedNum { get; init; } = 0;

        public long SuccessRate { get;init; } = 0;

        public long FailedRate { get; init; } = 0;

        public DateTime ExecutedAt { get; init; } = DateTime.UtcNow;
    }
}
