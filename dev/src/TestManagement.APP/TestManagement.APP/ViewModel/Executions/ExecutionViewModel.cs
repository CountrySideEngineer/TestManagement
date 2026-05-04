using TestManagement.APP.Dto.TestExecution;

namespace TestManagement.APP.ViewModel.Executions
{
    public class ExecutionViewModel
    {
        /// <summary>
        /// The identifier of the created test execution record.
        /// </summary>
        public long TestExecutionId { get; set; } = 0;

        /// <summary>
        /// The UTC date and time when the test execution occurred.
        /// </summary>
        public DateTime ExecutedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// The target environment for the test execution (e.g. "staging", "production").
        /// </summary>
        public string Environment { get; set; } = null!;

        /// <summary>
        /// The revision identifier (commit hash or build number) associated with the execution.
        /// </summary>
        public string Revision { get; set; } = string.Empty;

        /// <summary>
        /// The number of test cases that resulted in success during the latest execution.
        /// </summary>
        public long PassedNum { get; init; } = 0;

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
        public long SuccessRate { get; init; } = 0;

        /// <summary>
        /// The failure rate of the latest execution expressed as a percentage (0-100).
        /// </summary>
        public long FailedRate { get; init; } = 0;
    }
}
