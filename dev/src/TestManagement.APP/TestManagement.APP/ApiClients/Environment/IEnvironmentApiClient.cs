using TestManagement.APP.Dto.Environment.Get;
using TestManagement.APP.Dto.Environment.Post;

namespace TestManagement.APP.ApiClients.Environment
{
    public interface IEnvironmentApiClient
    {
        Task<IList<GetEnvironmentResponse>> GetEnvironmentsAsync();
        Task<IList<GetEnvironmentResponse>> GetEnvironmentsByNameAsync(GetEnvironmentRequest request);
        Task<PostEnvironmentResponse?> CreateEnvironmentAsync(PostEnvironmentRequest request);
    }
}
