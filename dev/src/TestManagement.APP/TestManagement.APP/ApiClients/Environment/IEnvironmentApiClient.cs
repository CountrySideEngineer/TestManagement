using TestManagement.APP.Dto.Environment.Get;

namespace TestManagement.APP.ApiClients.Environment
{
    public interface IEnvironmentApiClient
    {
        Task<IList<GetEnvironmentResponse>> GetEnvironmentsAsync();
    }
}
