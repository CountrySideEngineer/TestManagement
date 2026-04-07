using TestManagement.APP.Dto.TestExecution.Get;

namespace TestManagement.APP.ApiClients
{
    public interface ITestExecutionApiClient
    {
        Task<IList<GetTestExecutionResponse>?> GetTestExecutionsAsync();
    }
}
