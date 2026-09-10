using TestManagement.API.Models;

namespace TestManagement.API.Services
{
    /// <summary>
    /// Service interface for managing Project &amp; TestSuite compositions (N:N).
    /// </summary>
    public interface IProjectCompositionService
    {
        /// <summary>
        /// Returns all project compositions.
        /// </summary>
        Task<ICollection<ProjectComposition>> GetAllAsync(CancellationToken ct);

        /// <summary>
        /// Returns compositions for a specific project.
        /// </summary>
        Task<ICollection<ProjectComposition>> GetByProjectIdAsync(long projectId, CancellationToken ct);

        /// <summary>
        /// Gets a single composition by id.
        /// </summary>
        Task<ProjectComposition?> GetByIdAsync(long id, CancellationToken ct);

        /// <summary>
        /// Creates a new project composition linking a project and a test suite.
        /// </summary>
        Task<ProjectComposition> CreateAsync(long projectId, long testSuiteId, CancellationToken ct);

        /// <summary>
        /// Deletes a composition by id.
        /// </summary>
        Task<bool> DeleteAsync(long id, CancellationToken ct);
    }
}