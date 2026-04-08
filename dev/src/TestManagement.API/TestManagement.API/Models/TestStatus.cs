namespace TestManagement.API.Models
{
    /// <summary>
    /// Represents the status of a test result (for example: Passed, Failed, Skipped).
    /// Contains metadata used to determine if the status indicates success or is terminal.
    /// </summary>
    public class TestStatus
    {
        /// <summary>
        /// Primary key identifier for the test status.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Machine-friendly code for the status (e.g. "PASSED", "FAILED").
        /// </summary>
        public string Code { get; set; } = null!;

        /// <summary>
        /// Human-readable display name for the status.
        /// </summary>
        public string DisplayName { get; set; } = null!;

        /// <summary>
        /// Indicates whether this status should be considered a successful outcome.
        /// </summary>
        public bool IsSuccess { get; set; }

        public bool IsFailed { get; set; }

        public bool IsSkipped { get; set; }

        public bool IsExcluded { get; set; }

        public bool IsInProgress { get; set; }

        /// <summary>
        /// Indicates whether this status is terminal for a test's lifecycle (no further transitions expected).
        /// </summary>
        public bool IsTerminal { get; set; }

        /// <summary>
        /// Sort order used when listing statuses in a UI or report.
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// Timestamp when the status was created (UTC).
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Timestamp when the status was last updated (UTC).
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Navigation property to related test results that have this status.
        /// </summary>
        public ICollection<TestResult> TestResults { get; set; } = new List<TestResult>();
    }
}
