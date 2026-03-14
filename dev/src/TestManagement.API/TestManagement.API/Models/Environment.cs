namespace TestManagement.API.Models
{
    /// <summary>
    /// Represents a test execution environment (for example: OS and runtime information).
    /// </summary>
    public class Environment
    {
        /// <summary>
        /// Unique identifier for the environment.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Human-readable name for the environment (e.g. "Windows Server 2022 - Prod").
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Operating system details for the environment (optional).
        /// </summary>
        public string? Os { get; set; }

        /// <summary>
        /// Runtime or framework information for the environment (optional).
        /// </summary>
        public string? RunTime { get; set; }

        /// <summary>
        /// The point in time when this record was created (UTC).
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// The point in time when this record was last updated (UTC).
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Collection of test execution items that were run in this environment.
        /// </summary>
        public ICollection<TestExecutionItem> TestExecutionItems { get; set; } = new List<TestExecutionItem>();
    }
}
