namespace TestManagement.API.Features.TestResult.Get
{
    public class GetTestResultResponse
    {
        /// <summary>
        /// Primary identifier of the test result record.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Identifier of the test execution item that produced this result.
        /// </summary>
        public long TestExecutionItemId { get; set; }

        /// <summary>
        /// Identifier of the specific test case version that was executed.
        /// </summary>
        public long TestCaseVersionId { get; set; }

        /// <summary>
        /// Identifier of the parent test case for the executed version.
        /// </summary>
        public long TestCaseId { get; set; }

        /// <summary>
        /// Human-friendly name of the test case version.
        /// </summary>
        public string TestCaseVersionName { get; set; } = string.Empty;

        /// <summary>
        /// Numeric version number of the test case version.
        /// </summary>
        public long TestCaseVersionNumber { get; set; }

        /// <summary>
        /// Optional identifier of the test level/category associated with the test case version.
        /// </summary>
        public long? TestLevelId { get; set; }

        /// <summary>
        /// Display name of the test level (for example: "unit", "integration").
        /// </summary>
        public string TestLevelName { get; set; } = string.Empty;

        /// <summary>
        /// Machine-friendly code for the test level used for lookups or filtering.
        /// </summary>
        public string TestLevelCode { get; set; } = string.Empty;

        /// <summary>
        /// Machine-friendly status code representing the result outcome (e.g. "passed").
        /// </summary>
        public string StatusCode { get; set; } = string.Empty;

        /// <summary>
        /// Human-readable display name for the status (for example: "Passed").
        /// </summary>
        public string StatusDisplayName { get; set; } = string.Empty;

        /// <summary>
        /// Optional message or details produced during test execution (errors, logs, notes).
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// The actual outcome or output produced by the test execution.
        /// </summary>
        public string ActualResult { get; set; } = string.Empty;

        /// <summary>
        /// UTC timestamp when the test was executed.
        /// </summary>
        public DateTime ExecutedAt { get; set; }

        /// <summary>
        /// UTC timestamp when this test result record was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// UTC timestamp when this test result record was last updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
}
