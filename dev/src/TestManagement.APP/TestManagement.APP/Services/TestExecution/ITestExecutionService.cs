using TestManagement.APP.Dto.TestExecution.Get;
using TestManagement.APP.Dto.TestExecution.Post;
using TestManagement.APP.ViewModel.Executions;

namespace TestManagement.APP.Services.TestExecution
{
    public interface ITestExecutionService
    {
        Task<ICollection<ExecutionIndexViewModel>?> GetExecutionsAsync();

        Task<ICollection<GetTestExecutionResponse>?> GetTestExecutionsAsync();

        Task<global::TestManagement.APP.Dto.TestExecution.Post.PostTestExecutionResponse?> CreateExecutionAsync(DateTime executedAt, string environment, string revision);
    }
}
