namespace TestManagement.APP.Dto.TestResult
{
    /// <summary>
    /// Represents a parsed test result with details about the test case execution.
    /// </summary>
    public class ParsedTestResult
    {
        /// <summary>
        /// Gets or sets the unique code identifying the test result.
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the test result.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the test result,
        /// providing additional details about the test case and its execution.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the status of the test result,
        /// indicating whether the test case passed, failed, or was skipped.
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the timestamp indicating when the test case was executed,
        /// </summary>
        public DateTime ExecutedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets a value indicating
        /// whether the test case execution resulted in a failure,
        /// </summary>
        public bool IsFailed { get; set; }
    }
}
