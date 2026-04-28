using TestManagement.APP.Dto.TestExecution.Get;
using TestManagement.APP.Dto.TestExecution.Post;
using TestManagement.APP.Dto.TestExecution.Get;

namespace TestManagement.APP.ApiClients
{
    public interface ITestExecutionApiClient
    {
        Task<IList<GetTestExecutionResponse>?> GetTestExecutionsAsync();
        Task<global::TestManagement.APP.Dto.TestExecution.Post.PostTestExecutionResponse?> CreateTestExecutionAsync(global::TestManagement.APP.Dto.TestExecution.Post.PostTestExecutionRequest request);
    }
}
