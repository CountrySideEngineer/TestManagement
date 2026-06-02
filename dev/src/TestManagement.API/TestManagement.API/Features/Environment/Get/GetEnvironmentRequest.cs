namespace TestManagement.API.Features.Environment.Get
{
    /// <summary>
    /// Request DTO used to query environments. All properties are optional and can be used
    /// to filter environments by name, operating system, or runtime information.
    /// </summary>
    public class GetEnvironmentRequest
    {
        /// <summary>
        /// Optional environment name to filter by.
        /// </summary>
        public string? Name { get; set; } = null!;

        /// <summary>
        /// Optional operating system string to filter by (e.g. "Windows Server 2022").
        /// </summary>
        public string? Os { get; set; } = null!;

        /// <summary>
        /// Optional runtime/framework string to filter by (e.g. ".NET 8").
        /// </summary>
        public string? RunTime { get; set; } = null!;
    }
}
