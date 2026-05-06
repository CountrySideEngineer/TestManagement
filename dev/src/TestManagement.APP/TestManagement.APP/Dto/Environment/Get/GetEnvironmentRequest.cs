namespace TestManagement.APP.Dto.Environment.Get
{
    /// <summary>
    /// Request DTO used to specify criteria for retrieving environments by name.
    /// </summary>
    public class GetEnvironmentRequest
    {
        /// <summary>
        /// The environment name to query for.
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}
