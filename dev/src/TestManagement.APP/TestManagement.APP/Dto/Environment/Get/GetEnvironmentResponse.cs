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
