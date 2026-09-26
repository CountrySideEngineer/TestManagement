using TestManagement.API.Features.Project.Create;
using TestManagement.API.Features.Project.Get;
using TestManagement.API.Features.TestSuite.Get;
using TestManagement.API.Models;

namespace TestManagement.API.Services
{
    /// <summary>
    /// Service interface for managing Project &amp; TestSuite compositions (N:N).
    /// </summary>
    public interface IProjectTestSuiteCompositionService
    {
        /// <summary>
        /// Returns all project and test suite compositions.
        /// </summary>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>A collection of test suite responses.</returns>
        Task<ICollection<GetTestSuiteResponse>> GetAllAsync(CancellationToken ct);

        /// <summary>
        /// Returns all compositions for a specific project.
        /// </summary>
        /// <param name="projectId">Identifier of the project.</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>The project's compositions, or null when no collection is available.</returns>
        Task<ICollection<ProjectTestSuiteComposition>?> GetByProjectIdAsync(long projectId, CancellationToken ct);

        /// <summary>
        /// Gets a single composition by its identifier.
        /// </summary>
        /// <param name="id">Identifier of the composition.</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>The matching composition, or null when it does not exist.</returns>
        Task<ProjectTestSuiteComposition?> GetByIdAsync(long id, CancellationToken ct);

        /// <summary>
        /// Creates a new project composition linking a project and a test suite.
        /// </summary>
        /// <param name="request">Request containing the project and test suite identifiers.</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>The response for the created composition.</returns>
        Task<CreateProjectTestSuiteCompositionResponse> CreateAsync(
            CreateProjectTestSuiteCompositionRequest request,
            CancellationToken ct);

        /// <summary>
        /// Creates multiple project and test suite compositions in one operation.
        /// </summary>
        /// <param name="requests">Requests containing the project and test suite identifiers.</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>The responses for the created compositions.</returns>
        Task<ICollection<CreateProjectTestSuiteCompositionResponse>> CreateAsync(
            ICollection<CreateProjectTestSuiteCompositionRequest> requests,
            CancellationToken ct);

        /// <summary>
        /// Deletes a composition by id.
        /// </summary>
        /// <param name="id">Identifier of the composition to delete.</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>True when the composition was deleted; otherwise false.</returns>
        Task<bool> DeleteAsync(long id, CancellationToken ct);
    }
}