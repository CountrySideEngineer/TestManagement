namespace TestManagement.APP.Dto.TestResult.Register
{
    public class RegisterTestResultResponse
    {
        /// <summary>
        /// Test result identifier generated upon successful registration of the test result.
        /// </summary>
        public long TestResultId { get; set; }

        /// <summary>
        /// Foreign key referencing the test execution item that triggered this result.
        /// </summary>
        public long TestExecutionItemId { get; set; }

        /// <summary>
        /// Test case identifier associated with this test result.
        /// This is used to link the result to the specific test case executed.
        /// </summary>
        public long TestCaseId { get; set; } = 0;

        /// <summary>
        /// Foreign key referencing the specific <see cref="TestCaseVersion"/> executed.
        /// </summary>
        public long TestCaseVersionNumber { get; set; } = 0;

        /// <summary>
        /// Identifier for the test level/category associated with this test result.
        /// </summary>
        public long TestLevelId { get; set; } = 0;

        /// <summary>
        /// Optional message or details associated with the test execution (for example, error stack trace or additional notes).
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Status of the test result, indicating whether the test case passed, failed, or was skipped.
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Timestamp when the test was executed (UTC).
        /// </summary>
        public DateTime ExecutedAt { get; set; } = DateTime.UtcNow;
    }
}
