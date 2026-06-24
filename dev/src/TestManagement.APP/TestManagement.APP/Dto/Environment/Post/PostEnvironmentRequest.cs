namespace TestManagement.APP.Dto.Environment.Post
{
    /// <summary>
    /// Request DTO for creating a new environment entry in the API.
    /// All properties are represented as plain strings.
    /// </summary>
    public class PostEnvironmentRequest
    {
        /// <summary>
        /// The display name of the environment.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The operating system name for the environment (e.g. "Windows", "Ubuntu").
        /// </summary>
        public string Os { get; set; } = string.Empty;

        /// <summary>
        /// The runtime version string for the environment (e.g. ".NET 8.0", "Node 18").
        /// </summary>
        public string RunTime { get; set; } = string.Empty;
    }
}
