using TestManagement.APP.Dto.Environment.Get;
using TestManagement.APP.Dto.Environment.Post;

namespace TestManagement.APP.ApiClients.Environment
{
    /// <summary>
    /// Client interface for environment-related operations against the remote Test API.
    /// Implementations are responsible for HTTP communication and JSON (de)serialization.
    /// </summary>
    public interface IEnvironmentApiClient
    {
        /// <summary>
        /// Retrieves all environments from the API.
        /// </summary>
        Task<IList<GetEnvironmentResponse>> GetEnvironmentsAsync();

        /// <summary>
        /// Retrieves environments that match the provided name.
        /// </summary>
        /// <param name="request">Request containing the name to filter by.</param>
        Task<IList<GetEnvironmentResponse>> GetEnvironmentsByNameAsync(GetEnvironmentRequest request);

        /// <summary>
        /// Creates a new environment entry in the remote API.
        /// </summary>
        /// <param name="request">The environment data to create.</param>
        Task CreateEnvironmentAsync(PostEnvironmentRequest request);
    }
}
