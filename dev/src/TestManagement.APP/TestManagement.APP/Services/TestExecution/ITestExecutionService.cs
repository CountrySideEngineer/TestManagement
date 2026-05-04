using TestManagement.APP.Dto.TestExecution.Get;
using TestManagement.APP.Dto.TestExecution.Post;
using TestManagement.APP.ViewModel.Executions;

namespace TestManagement.APP.Services.TestExecution
{
    public interface ITestExecutionService
    {
        Task<ICollection<ExecutionViewModel>?> GetExecutionsAsync();

        Task<ICollection<GetTestExecutionResponse>?> GetTestExecutionsAsync();

        Task<ExecutionViewModel?> GetTestExecutionByIdAsync(long testExecutionId);

        Task<PostTestExecutionResponse?> CreateExecutionAsync(DateTime executedAt, string environment, string revision);
    }
}
