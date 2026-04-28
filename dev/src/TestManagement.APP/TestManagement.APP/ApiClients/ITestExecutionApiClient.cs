using TestManagement.APP.Dto.TestExecution.Get;
using TestManagement.APP.Dto.TestExecution.Post;

namespace TestManagement.APP.ApiClients
{
    public interface ITestExecutionApiClient
    {
        Task<IList<GetTestExecutionResponse>?> GetTestExecutionsAsync();
        Task<PostTestExecutionResponse?> CreateTestExecutionAsync(PostTestExecutionRequest request);
    }
}
