namespace TestManagement.API.Features.Environment.Get
{
    /// <summary>
    /// Response DTO returned when querying environment information.
    /// Contains the environment name and specific version's OS and runtime values.
    /// </summary>
    public class GetEnvironmentResponse
    {
        /// <summary>
        /// Name of the environment.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Operating system string for the returned environment version.
        /// </summary>
        public string Os { get; set; } = null!;

        /// <summary>
        /// Runtime/framework string for the returned environment version.
        /// </summary>
        public string RunTime { get; set; } = null!;

        /// <summary>
        /// Version number for the returned environment version.
        /// </summary>
        public long VersionNumber { get; set; } = 0;
    }
}
