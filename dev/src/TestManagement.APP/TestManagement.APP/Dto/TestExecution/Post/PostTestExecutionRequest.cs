namespace TestManagement.APP.Dto.TestExecution.Post
{
    /// <summary>
    /// Request DTO used to create a new test execution record via the API.
    /// Contains the execution timestamp, target environment and revision identifier.
    /// </summary>
    public class PostTestExecutionRequest
    {
        public DateTime ExecutedAt { get; set; } = DateTime.UtcNow;

        public string Environment { get; set; } = string.Empty;

        public string Revision { get; set; } = string.Empty;
    }
}
