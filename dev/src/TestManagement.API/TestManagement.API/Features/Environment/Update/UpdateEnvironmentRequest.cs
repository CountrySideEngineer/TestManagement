namespace TestManagement.API.Features.Environment.Update
{
    /// <summary>
    /// Request DTO used to update an environment by creating a new version.
    /// </summary>
    public class UpdateEnvironmentRequest
    {
        /// <summary>
        /// Name of the environment to update.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Operating system string for the new version.
        /// </summary>
        public string Os { get; set; } = string.Empty;

        /// <summary>
        /// Runtime/framework string for the new version.
        /// </summary>
        public string RunTime { get; set; } = string.Empty;
    }
}
