using TestManagement.APP.Dto.TestLevel.Get;

namespace TestManagement.APP.ApiClients.TestLevel
{
    public interface ITestLevelApiClient
    {
        Task<IList<GetTestLevelResponse>> GetTestLevelAsync();
    }
}
