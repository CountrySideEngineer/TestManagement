namespace TestManagement.APP.Dto.Environment.Get
{
    /// <summary>
    /// DTO returned by the API that represents an environment where tests can run.
    /// Contains basic metadata such as name, OS, runtime and a version identifier.
    /// </summary>
    public class GetEnvironmentResponse
    {
        /// <summary>
        /// Unique identifier of the environment record.
        /// </summary>
        public long EnvironmentId { get; set; } = 0;

        /// <summary>
        /// Human readable name of the environment (e.g. "staging", "windows-lab").
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Operating system name/version for the environment.
        /// </summary>
        public string Os { get;set; } = string.Empty;

        /// <summary>
        /// Runtime or platform information (for example .NET runtime version).
        /// </summary>
        public string RunTime { get; set; } = string.Empty;

        /// <summary>
        /// Numeric version used to track changes to the environment specification.
        /// </summary>
        public long Version { get; set; } = 0;
    }
}
