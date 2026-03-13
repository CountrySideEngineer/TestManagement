namespace TestManagement.API.Models
{
    /// <summary>
    /// Represents a test execution which groups together execution items run in a specific environment.
    /// </summary>
    public class TestExecution
    {
        /// <summary>
        /// Unique identifier for the test execution.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Revision or version identifier for the execution (e.g. commit hash or build number).
        /// </summary>
        public string Revision { get; set; } = null!;

        /// <summary>
        /// Identifier of the environment where the tests were executed.
        /// </summary>
        public long EnvironmentId { get; set; }

        /// <summary>
        /// The point in time when the tests were executed (UTC).
        /// </summary>
        public DateTime ExecutedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// The point in time when this record was created (UTC).
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Navigation property to the environment details associated with this execution.
        /// </summary>
        public Environment Environment { get; set; } = null!;

        // Backing field for the collection of execution items.
        private readonly List<TestExecutionItem> _items = new();

        /// <summary>
        /// Read-only collection of individual test execution items that belong to this execution.
        /// </summary>
        public IReadOnlyCollection<TestExecutionItem> Items => _items;
    }
}
