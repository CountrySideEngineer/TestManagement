using TestManagement.APP.Dto.TestExecution.Get;
using TestManagement.APP.ViewModel.Executions;

namespace TestManagement.APP.Services
{
    public interface ITestExecutionService
    {
        Task<ICollection<ExecutionIndexViewModel>?> GetExecutionsAsync();

        Task<ICollection<GetTestExecutionResponse>?> GetTestExecutionsAsync();
    }
}
