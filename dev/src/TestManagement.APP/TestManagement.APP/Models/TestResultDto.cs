namespace TestManagement.APP.Models
{
    /// <summary>
    /// Enumeration representing possible outcomes for a test execution.
    /// </summary>
    public enum TestStatus
    {
        Unknown = 0,
        Success = 1,
        Failure = 2,
        Skipped = 3,
        Blocked = 4
    }

    /// <summary>
    /// Data transfer object that represents the result of a single test execution.
    /// </summary>
    public class TestResultDto
    {
        /// <summary>
        /// Primary identifier for the test result.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The actual output or message produced by the test run.
        /// </summary>
        public string ActualResult { get; set; } = string.Empty;

        /// <summary>
        /// Foreign key to the parent test run.
        /// </summary>
        public int TestRunId { get; set; }

        /// <summary>
        /// Navigation property to the associated <see cref="TestRunDto"/>.
        /// </summary>
        public TestRunDto TestRun { get; set; } = new();

        /// <summary>
        /// Foreign key to the related test case.
        /// </summary>
        public int TestCaseId { get; set; }

        /// <summary>
        /// Navigation property to the associated <see cref="TestCaseDto"/>.
        /// </summary>
        public TestCaseDto TestCase { get; set; } = new();

        /// <summary>
        /// Result status of the test execution.
        /// </summary>
        public TestStatus Status { get; set; } = TestStatus.Unknown;

        /// <summary>
        /// UTC timestamp when the test was executed.
        /// </summary>
        public DateTime ExecutedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// UTC timestamp when the test result record was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// UTC timestamp when the test result record was last updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
