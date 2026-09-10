using Microsoft.EntityFrameworkCore;
using TestManagement.API.Data;
using TestManagement.API.Models;

namespace TestManagement.API.Services
{
    /// <summary>
    /// Service that provides operations to manage ProjectComposition entities.
    /// </summary>
    public class ProjectCompositionService : IProjectCompositionService
    {
        private readonly TestManagementDbContext _context;
        private readonly ILogger<ProjectCompositionService>? _logger;

        /// <summary>
        /// Creates a new instance of <see cref="ProjectCompositionService"/>.
        /// </summary>
        public ProjectCompositionService(
            TestManagementDbContext context,
            ILogger<ProjectCompositionService>? logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all <see cref="ProjectComposition"/> entities from the database.
        /// This returns a no-tracking collection suitable for read-only scenarios.
        /// </summary>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>A collection of <see cref="ProjectComposition"/>.</returns>
        public virtual async Task<ICollection<ProjectComposition>> GetAllAsync(CancellationToken ct)
        {
            _logger?.LogDebug("ProjectCompositionService::GetAllAsync() start");
            return await _context.ProjectCompositions
                .AsNoTracking()
                .ToListAsync(ct);
        }

        /// <summary>
        /// Retrieves all <see cref="ProjectComposition"/> entities for the specified project id.
        /// The returned collection is not tracked by the DbContext.
        /// </summary>
        /// <param name="projectId">Identifier of the project whose compositions are returned.</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>A collection of <see cref="ProjectComposition"/> that belong to the project.</returns>
        public virtual async Task<ICollection<ProjectComposition>> GetByProjectIdAsync(long projectId, CancellationToken ct)
        {
            _logger?.LogDebug("ProjectCompositionService::GetByProjectIdAsync({ProjectId}) start", projectId);
            return await _context.ProjectCompositions
                .Where(pc => pc.ProjectId == projectId)
                .AsNoTracking()
                .ToListAsync(ct);
        }

        /// <summary>
        /// Retrieves a single <see cref="ProjectComposition"/> by its identifier.
        /// Returns null when no matching entity is found.
        /// </summary>
        /// <param name="id">Identifier of the composition to retrieve.</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>The matching <see cref="ProjectComposition"/> or null if not found.</returns>
        public virtual async Task<ProjectComposition?> GetByIdAsync(long id, CancellationToken ct)
        {
            _logger?.LogDebug("ProjectCompositionService::GetByIdAsync({Id}) start", id);
            return await _context.ProjectCompositions
                .Where(pc => pc.Id == id)
                .AsNoTracking()
                .FirstOrDefaultAsync(ct);
        }

        /// <summary>
        /// Creates a new <see cref="ProjectComposition"/> linking the specified project and test suite.
        /// Validates that both the project and the test suite exist and prevents creating duplicate links.
        /// Throws <see cref="InvalidOperationException"/> when validation fails.
        /// </summary>
        /// <param name="projectId">Identifier of the project to link.</param>
        /// <param name="testSuiteId">Identifier of the test suite to link.</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>The created <see cref="ProjectComposition"/>.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the project or test suite does not exist or the composition already exists.</exception>
        public virtual async Task<ProjectComposition> CreateAsync(long projectId, long testSuiteId, CancellationToken ct)
        {
            _logger?.LogDebug("ProjectCompositionService::CreateAsync(projectId={ProjectId}, testSuiteId={TestSuiteId}) start", projectId, testSuiteId);

            // Ensure related entities exist
            var projectExists = await _context.Projects.AnyAsync(p => p.Id == projectId, ct);
            if (!projectExists)
            {
                throw new InvalidOperationException($"Project with id {projectId} does not exist.");
            }

            var testSuiteExists = await _context.TestSuites.AnyAsync(ts => ts.Id == testSuiteId, ct);
            if (!testSuiteExists)
            {
                throw new InvalidOperationException($"TestSuite with id {testSuiteId} does not exist.");
            }

            // Prevent duplicate (unique index exists at DB level)
            var already = await _context.ProjectCompositions
                .AnyAsync(pc => pc.ProjectId == projectId && pc.TestSuiteId == testSuiteId, ct);
            if (already)
            {
                throw new InvalidOperationException("The composition already exists.");
            }

            var composition = new ProjectComposition
            {
                ProjectId = projectId,
                TestSuiteId = testSuiteId
            };

            _context.ProjectCompositions.Add(composition);

            try
            {
                await _context.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex)
            {
                _logger?.LogError(ex, "Failed to create ProjectComposition(projectId={ProjectId}, testSuiteId={TestSuiteId})", projectId, testSuiteId);
                throw;
            }

            return composition;
        }

        /// <summary>
        /// Deletes a <see cref="ProjectComposition"/> by its identifier.
        /// </summary>
        /// <param name="id">Identifier of the composition to delete.</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>True if the composition was found and deleted; otherwise false.</returns>
        public virtual async Task<bool> DeleteAsync(long id, CancellationToken ct)
        {
            _logger?.LogDebug("ProjectCompositionService::DeleteAsync({Id}) start", id);

            var existing = await _context.ProjectCompositions
                .FirstOrDefaultAsync(pc => pc.Id == id, ct);
            if (existing == null)
            {
                return false;
            }

            _context.ProjectCompositions.Remove(existing);
            await _context.SaveChangesAsync(ct);
            return true;
        }
    }
}