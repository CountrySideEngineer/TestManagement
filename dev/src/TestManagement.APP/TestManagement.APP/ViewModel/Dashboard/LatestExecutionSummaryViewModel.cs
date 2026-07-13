using Microsoft.AspNetCore.Hosting.Server;

namespace TestManagement.APP.ViewModel.Dashboard
{
    public class LatestExecutionSummaryViewModel
    {
        /// <summary>
        /// The unique identifier of the execution.
        /// </summary>
        public long ExecutionId { get; init; } = 0;

        /// <summary>
        /// The source control revision identifier associated with the latest execution.
        /// </summary>
        public string Revision { get; init; } = string.Empty;

        /// <summary>
        /// The name of the environment where the execution ran (e.g. "staging", "production").
        /// </summary>
        public string EnvironmentName { get; init; } = string.Empty;

        /// <summary>
        /// The number of test cases that resulted in errors during the latest execution.
        /// </summary>
        public long ErrorNum { get; init; } = 0;

        /// <summary>
        /// The number of test cases that were skipped in the latest execution.
        /// </summary>
        public long SkippedNum { get; init; } = 0;

        /// <summary>
        /// The number of test cases that were excluded from the latest execution.
        /// </summary>
        public long ExcludedNum { get; init; } = 0;

        /// <summary>
        /// The total number of test cases that were executed.
        /// </summary>
        public long ExecutedNum { get; init; } = 0;

        /// <summary>
        /// The success rate of the latest execution expressed as a percentage (0-100).
        /// </summary>
        public long SuccessRate { get;init; } = 0;

        /// <summary>
        /// The failure rate of the latest execution expressed as a percentage (0-100).
        /// </summary>
        public long FailedRate { get; init; } = 0;

        /// <summary>
        /// The UTC date and time when the latest execution was performed.
        /// </summary>
        public DateTime ExecutedAt { get; init; } = DateTime.UtcNow;
    }
}
