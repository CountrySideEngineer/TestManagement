namespace TestManagement.API.Features.Environment.Update
{
    /// <summary>
    /// Response DTO returned after updating an environment (adding a new version).
    /// Contains the environment name and the created version's OS, runtime and version number.
    /// </summary>
    public class UpdateEnvironmentResponse
    {
        /// <summary>
        /// Name of the environment that was updated.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Operating system string for the created environment version.
        /// </summary>
        public string Os { get; set; } = string.Empty;

        /// <summary>
        /// Runtime/framework string for the created environment version.
        /// </summary>
        public string RunTime { get; set; } = string.Empty;

        /// <summary>
        /// Version number assigned to the newly created environment version.
        /// </summary>
        public long VersionNumber { get; set; } = 0;
    }
}
