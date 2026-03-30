namespace TestManagement.API.Models
{
    public class EnvironmentVersion
    {
        public long Id { get; set; } = 0;

        /// <summary>
        /// Operating system details for the environment (optional).
        /// </summary>
        public string Os { get; set; } = string.Empty;

        /// <summary>
        /// Runtime or framework information for the environment (optional).
        /// </summary>
        public string RunTime { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public long EnvironmentId { get; set; } = 0;

        public long VersionNumber { get; set; } = 0;

        public bool IsLatest { get; set; } = false;  

        /// <summary>
        /// Collection of test execution items that were run in this environment.
        /// </summary>
        public ICollection<TestExecutionItem> TestExecutionItems { get; set; } = new List<TestExecutionItem>();
    }
}
