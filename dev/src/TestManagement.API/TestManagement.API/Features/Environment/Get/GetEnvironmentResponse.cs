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

        /// <summary>
        /// Version number for the returned environment version.
        /// </summary>
        public long VersionNumber { get; set; } = 0;

        /// <summary>
        /// Identifier of the specific environment version record.
        /// This can be used to reference the underlying version entity directly.
        /// </summary>
        public long VersionId { get; set; } = 0;

        public class EnvironmentVersion
        {
            /// <summary>
            /// Identifier of the specific environment version record.
            /// This can be used to reference the underlying version entity directly.
            /// </summary>
            public long VersionId { get; set; } = 0;

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


}
