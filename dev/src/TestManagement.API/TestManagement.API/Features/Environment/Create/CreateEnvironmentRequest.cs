namespace TestManagement.API.Features.Environment.Create
{
    /// <summary>
    /// Request DTO used to create a new environment and its initial version.
    /// </summary>
    public class CreateEnvironmentRequest
    {
        /// <summary>
        /// Human-readable name for the environment (should be unique).
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Operating system information for the environment (e.g. "Windows Server 2022").
        /// </summary>
        public string Os { get; set; } = string.Empty;

        /// <summary>
        /// Runtime or framework information for the environment (e.g. ".NET 8").
        /// </summary>
        public string RunTime { get; set; } = string.Empty;
    }
}
