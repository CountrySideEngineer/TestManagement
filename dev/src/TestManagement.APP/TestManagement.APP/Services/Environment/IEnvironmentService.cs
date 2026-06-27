using TestManagement.APP.Dto.Environment.Create;
using TestManagement.APP.Dto.Environment.Get;
using TestManagement.APP.ViewModel.Environment;

namespace TestManagement.APP.Services.Environment
{
    /// <summary>
    /// Interface for a service that manages environment information.
    /// Provides methods to retrieve environment data.
    /// </summary>
    public interface IEnvironmentService
    {
        /// <summary>
        /// Retrieves all available environments asynchronously.
        /// </summary>
        /// <returns>A collection of <see cref="EnvironmentViewModel"/> objects, or null if the operation fails.</returns>
        Task<ICollection<EnvironmentViewModel>?> GetEnvironmentsAsync();

        /// <summary>
        /// Retrieves all environments matching the specified name asynchronously.
        /// </summary>
        /// <param name="name">The name of the environment to retrieve.</param>
        /// <returns>A collection of <see cref="EnvironmentViewModel"/> objects matching the name, or null if not found.</returns>
        Task<ICollection<EnvironmentViewModel>?> GetEnvironmentsByNameAsync(string name);

        /// <summary>
        /// Retrieves the latest environment with the specified name asynchronously.
        /// </summary>
        /// <param name="name">The name of the environment to retrieve.</param>
        /// <returns>The latest <see cref="EnvironmentViewModel"/> matching the name, or null if not found.</returns>
        Task<EnvironmentViewModel?> GetLatestEnvironmentByNameAsync(string name);

        /// <summary>
        /// Creates a new environment using the provided <see cref="CreateEnvironmentRequest"/>.
        /// </summary>
        /// <param name="request">The request containing the details of the environment to create.</param>
        /// <returns>
        /// A <see cref="CreateEnvironmentResponse"/> representing the created environment on success; otherwise <c>null</c> if the operation fails.
        /// </returns>
        Task<CreateEnvironmentResponse?> CreateEnvironmentAsync(CreateEnvironmentRequest request);
    }
}
