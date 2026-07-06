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

        public Environment.EnvironmentVersion Version { get; set; } = null!;
    }
}
