using TestManagement.APP.Dto.TestExecution.Get;

namespace TestManagement.APP.Dto.TestExecution.Post
{
    /// <summary>
    /// Response DTO returned by the API after creating a test execution.
    /// Contains the created execution information including the generated ID.
    /// </summary>
    public class PostTestExecutionResponse
    {
        public GetTestExecutionResponse CreatedExecution { get; set; } = new GetTestExecutionResponse();
    }
}
