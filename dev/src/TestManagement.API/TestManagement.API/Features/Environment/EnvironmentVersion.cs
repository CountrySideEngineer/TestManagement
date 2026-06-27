namespace TestManagement.API.Features.Environment
{
    public class EnvironmentVersion
    {
        /// <summary>
        /// Identifier of the specific environment version record.
        /// This can be used to reference the underlying version entity directly.
        /// </summary>
        public long Id { get; set; } = 0;

        /// <summary>
        /// Version number for the returned environment version.
        /// </summary>
        public long VersionNumber { get; set; } = 0;

        /// <summary>
        /// Operating system string for the returned environment version.
        /// </summary>
        public string Os { get; set; } = null!;

        /// <summary>
        /// Runtime/framework string for the returned environment version.
        /// </summary>
        public string RunTime { get; set; } = null!;

        /// <summary>
        /// Indicates whether this version is the latest available for the environment.
        /// </summary>
        public bool IsLatest { get; set; } = false;
    }
}
