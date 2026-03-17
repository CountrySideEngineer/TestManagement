namespace TestManagement.API.Models
{
    /// <summary>
    /// Represents a single test execution item which may contain multiple test results.
    /// </summary>
    public class TestExecutionItem
    {
        /// <summary>
        /// Unique identifier for the execution item.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Foreign key linking this item to its parent test execution.
        /// </summary>
        public long TestExecutionId { get; set; } = 0;

        /// <summary>
        /// The point in time when this specific item was executed (UTC).
        /// </summary>
        public DateTime ExecutedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Identifier of the environment where this item was executed.
        /// </summary>
        public long EnvironmentId { get; set; } = 0;

        /// <summary>
        /// The point in time when this record was created (UTC).
        /// </summary>
        public DateTime CreatedAt { get;set; } = DateTime.UtcNow;

        /// <summary>
        /// The point in time when this record was last updated (UTC).
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Collection of test results associated with this execution item.
        /// </summary>
        public ICollection<TestResult> TestResults { get; set; } = new List<TestResult>();

        /// <summary>
        /// Navigation property to the parent test execution.
        /// </summary>
        public TestExecution? TestExecution { get; set; } = null;

        /// <summary>
        /// Navigation property to the environment where this item was executed.
        /// </summary>
        public Environment? Environment { get; set; } = null;

    }
}
