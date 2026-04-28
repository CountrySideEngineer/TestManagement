namespace TestManagement.APP.Dto.TestExecution.Post
{
    /// <summary>
    /// Request DTO used to create a new test execution record via the API.
    /// Contains the execution timestamp, target environment and revision identifier.
    /// </summary>
    public class PostTestExecutionRequest
    {
        /// <summary>
        /// The UTC timestamp when the test execution occurred. Defaults to now (UTC) if not provided.
        /// </summary>
        public DateTime ExecutedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// The name or identifier of the environment where the tests were executed.
        /// </summary>
        public string Environment { get; set; } = string.Empty;

        /// <summary>
        /// Revision or commit identifier associated with the test run (e.g. git SHA).
        /// </summary>
        public string Revision { get; set; } = string.Empty;
    }
}
