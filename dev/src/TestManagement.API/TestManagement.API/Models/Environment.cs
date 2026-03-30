namespace TestManagement.API.Models
{
    /// <summary>
    /// Represents a test execution environment (for example: OS and runtime information).
    /// </summary>
    public class Environment
    {
        private readonly List<EnvironmentVersion> _versions = new List<EnvironmentVersion>();

        /// <summary>
        /// Unique identifier for the environment.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Human-readable name for the environment (e.g. "Windows Server 2022 - Prod").
        /// </summary>
        public string Name { get; set; } = null!;

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

        public IReadOnlyCollection<EnvironmentVersion> Versions => _versions;

        public virtual void AddVersion(string Os, string runTime)
        {
            EnvironmentVersion version = new EnvironmentVersion
            {
                Os = Os,
                RunTime = runTime,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                EnvironmentId = this.Id,
                IsLatest = true
            };

            if (_versions.Count < 1)
            {
                version.VersionNumber = 1;
            }
            else
            {
                try
                {

                }
                catch (Exception ex)
                when ((ex is ArgumentNullException) || (ex is InvalidOperationException))
                {
                    throw new InvalidOperationException("Failed to calculate new version number.", ex);
                }
                long newVersionNumber = Versions.Max(_ => _.VersionNumber) + 1;
                version.VersionNumber = newVersionNumber;
            }
            
            foreach (var versionItem in _versions)
            {
                versionItem.IsLatest = false;
            }
            _versions.Add(version);
        }
    }
}
