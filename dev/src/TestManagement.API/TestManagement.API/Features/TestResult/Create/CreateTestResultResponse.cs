namespace TestManagement.API.Features.TestResult.Create
{
    /// <summary>
    /// Response DTO returned after creating a test result. Contains identifiers
    /// and metadata about the created test result.
    /// </summary>
    public class CreateTestResultResponse
    {
        /// <summary>
        /// Primary key identifier for the created test result record.
        /// </summary>
        public long ResultId { get; set; } = 0;

        /// <summary>
        /// Foreign key referencing the test execution item that triggered this result.
        /// </summary>
        public long TestExecutionItemId { get; set; }

        /// <summary>
        /// Foreign key referencing the specific <see cref="TestCaseId"/> executed.
        /// </summary>
        public long TestCaseId { get; set; } = 0;

        /// <summary>
        /// Version number of the test case that was executed.
        /// </summary>
        public long TestCaseVersionNumber { get; set; } = 0;

        /// <summary>
        /// Identifier for the test level or environment (for example, unit, integration, system).
        /// </summary>
        public long TestLevelId { get; set; } = 0;

        /// <summary>
        /// Optional message or details associated with the test execution (for example, error stack trace or additional notes).
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Timestamp when the test was executed (UTC).
        /// </summary>
        public DateTime ExecutedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Status of the test result (for example, "Passed", "Failed", "Skipped").
        /// </summary>
        public string TestResultStatus { get; set; } = string.Empty;
    }
}
