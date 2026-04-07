using TestManagement.APP.Dto.TestExecution.Get;

namespace TestManagement.APP.Services
{
    public interface ITestExecutionService
    {
        Task<ICollection<GetTestExecutionResponse>?> GetTestExecutionsAsync();
    }
}
