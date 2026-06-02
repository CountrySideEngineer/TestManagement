using TestManagement.APP.Dto.TestExecution.Get;

namespace TestManagement.APP.Dto.TestExecution.Post
{
    /// <summary>
    /// Response DTO returned by the API after creating a test execution.
    /// Contains the created execution information including the generated ID.
    /// </summary>
    public class PostTestExecutionResponse
    {
        /// <summary>
        /// The created test execution returned by the API. Contains the generated id and other metadata.
        /// </summary>
        public GetTestExecutionResponse CreatedExecution { get; set; } = new GetTestExecutionResponse();
    }
}
