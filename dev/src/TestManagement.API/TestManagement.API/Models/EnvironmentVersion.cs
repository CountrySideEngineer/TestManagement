namespace TestManagement.API.Models
{
    /// <summary>
    /// Represents a specific version of an environment configuration (OS and runtime details).
    /// </summary>
    public class EnvironmentVersion
    {
        /// <summary>
        /// Primary key identifier for the environment version.
        /// </summary>
        public long Id { get; set; } = 0;

        /// <summary>
        /// Operating system details for the environment version (e.g. "Windows Server 2022").
        /// </summary>
        public string Os { get; set; } = string.Empty;

        /// <summary>
        /// Runtime or framework information for the environment version (e.g. ".NET 8").
        /// </summary>
        public string RunTime { get; set; } = string.Empty;

        /// <summary>
        /// The point in time when this environment version record was created (UTC).
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// The point in time when this environment version record was last updated (UTC).
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Foreign key referencing the parent environment.
        /// </summary>
        public long EnvironmentId { get; set; } = 0;

        /// <summary>
        /// Numeric version number for this environment version.
        /// </summary>
        public long VersionNumber { get; set; } = 0;

        /// <summary>
        /// Indicates whether this version is the latest version for the parent environment.
        /// </summary>
        public bool IsLatest { get; set; } = false;  

        /// <summary>
        /// Navigation property to the parent <see cref="Environment"/>.
        /// </summary>
        public Environment? Environment { get; set; }

        /// <summary>
        /// Collection of test execution items that were run in this environment version.
        /// </summary>
        public ICollection<TestExecutionItem> TestExecutionItems { get; set; } = new List<TestExecutionItem>();
    }
}
