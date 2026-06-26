namespace TestManagement.APP.Dto.Environment.Post
{
    /// <summary>
    /// Response DTO for creating a new environment entry in the API.
    /// </summary>
    public class PostEnvironmentResponse
    {
        /// <summary>
        /// The unique identifier of the environment.
        /// </summary>
        public long Id { get; set; } = 0;

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
