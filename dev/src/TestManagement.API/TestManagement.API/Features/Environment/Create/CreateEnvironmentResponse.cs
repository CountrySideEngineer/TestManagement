namespace TestManagement.API.Features.Environment.Create
{
    /// <summary>
    /// Response DTO returned after creating an environment and its initial version.
    /// </summary>
    public class CreateEnvironmentResponse
    {
        /// <summary>
        /// Identifier of the created environment version record.
        /// </summary>
        public long Id { get; set; } = 0;

        /// <summary>
        /// Environment name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Operating system of the created environment version.
        /// </summary>
        public string Os { get; set; } = string.Empty;

        /// <summary>
        /// Runtime/framework of the created environment version.
        /// </summary>
        public string RunTime { get; set; } = string.Empty;

        /// <summary>
        /// Version number assigned to the created environment version.
        /// </summary>
        public long VersionNumber { get; set; } = 0;
    }
}
