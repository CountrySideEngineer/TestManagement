namespace TestManagement.API.Features.Environment.Get
{
    /// <summary>
    /// Response DTO returned when querying environment information.
    /// Contains the environment name and specific version's OS and runtime values.
    /// </summary>
    public class GetEnvironmentResponse
    {
        /// <summary>
        /// Unique identifier of the environment record.
        /// </summary>
        public long Id { get; set; } = 0;

        /// <summary>
        /// Name of the environment.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Collection of versions associated with this environment.
        /// </summary>
        public IEnumerable<EnvironmentVersion> Versions { get; set; } = null!;
    }


}
