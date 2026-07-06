using TestManagement.APP.Dto.Environment.Get;
using TestManagement.APP.Dto.Environment.Post;

namespace TestManagement.APP.ApiClients.Environment
{
    public interface IEnvironmentApiClient
    {
        Task<ICollection<GetEnvironmentResponse>> GetEnvironmentsAsync();
        Task<ICollection<GetEnvironmentResponse>> GetEnvironmentsByNameAsync(GetEnvironmentRequest request);
        Task<PostEnvironmentResponse?> CreateEnvironmentAsync(PostEnvironmentRequest request);
    }
}
