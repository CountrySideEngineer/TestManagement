using TestManagement.API.Features.Environment.Create;
using TestManagement.API.Features.Environment.Get;
using TestManagement.API.Features.Environment.Update;

namespace TestManagement.API.Services
{
    /// <summary>
    /// Service for managing environments used in test executions.
    /// Provides methods to query, create and update environment entities.
    /// </summary>
    public interface IEnvironmentService
    {
        /// <summary>
        /// Retrieve all environments.
        /// </summary>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>A collection of <see cref="GetEnvironmentResponse"/> objects.</returns>
        Task<ICollection<GetEnvironmentResponse>> GetAllAsync(CancellationToken ct);

        /// <summary>
        /// Retrieve environments by identifier.
        /// </summary>
        /// <param name="id">Environment identifier.</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>A collection of <see cref="GetEnvironmentResponse"/> matching the id.</returns>
        Task<ICollection<GetEnvironmentResponse>> GetByIdAsync(int id, CancellationToken ct);

        /// <summary>
        /// Retrieve environments by name.
        /// </summary>
        /// <param name="name">Environment name to match.</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>A collection of <see cref="GetEnvironmentResponse"/> matching the name.</returns>
        Task<ICollection<GetEnvironmentResponse>> GetByNameAsync(string name, CancellationToken ct);

        /// <summary>
        /// Create a new environment.
        /// </summary>
        /// <param name="request">Creation request containing environment details.</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>The created environment response.</returns>
        Task<CreateEnvironmentResponse> CreateAsync(CreateEnvironmentRequest request, CancellationToken ct);

        /// <summary>
        /// Update an existing environment.
        /// </summary>
        /// <param name="request">Update request with modified environment fields.</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>The updated environment response.</returns>
        Task<UpdateEnvironmentResponse> UpdateAsync(UpdateEnvironmentRequest request, CancellationToken ct);
    }
}
