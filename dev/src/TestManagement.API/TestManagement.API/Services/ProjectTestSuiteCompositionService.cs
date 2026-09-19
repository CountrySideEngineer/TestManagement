using Microsoft.EntityFrameworkCore;
using TestManagement.API.Data;
using TestManagement.API.Features.TestSuite.Get;
using TestManagement.API.Models;

namespace TestManagement.API.Services
{
    /// <summary>
    /// Service that provides operations to manage ProjectComposition entities.
    /// </summary>
    public class ProjectTestSuiteCompositionService : IProjectTestSuiteCompositionService
    {
        private readonly TestManagementDbContext _context;
        private readonly ILogger<ProjectTestSuiteCompositionService>? _logger;

        /// <summary>
        /// Creates a new instance of <see cref="ProjectTestSuiteCompositionService"/>.
        /// </summary>
        public ProjectTestSuiteCompositionService(
            TestManagementDbContext context,
            ILogger<ProjectTestSuiteCompositionService>? logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all <see cref="ProjectTestSuiteComposition"/> entities from the database.
        /// This returns a no-tracking collection suitable for read-only scenarios.
        /// </summary>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>A collection of <see cref="ProjectTestSuiteComposition"/>.</returns>
        public virtual async Task<ICollection<GetTestSuiteResponse>> GetAllAsync(CancellationToken ct)
        {
            _logger?.LogDebug("ProjectCompositionService::GetAllAsync() start");

            var projectCompositions = await _context.ProjectTestSuiteCompositions
                .Include(_ => _.Project)
                .Include(_ => _.TestSuite)
                .AsNoTracking()
                .ToListAsync(ct);

            var testSuiteReponses = new List<GetTestSuiteResponse>();

            return testSuiteReponses;
        }

        /// <summary>
        /// Retrieves all <see cref="ProjectTestSuiteComposition"/> entities for the specified project id.
        /// The returned collection is not tracked by the DbContext.
        /// </summary>
        /// <param name="projectId">Identifier of the project whose compositions are returned.</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>A collection of <see cref="ProjectTestSuiteComposition"/> that belong to the project.</returns>
        public virtual async Task<ICollection<GetTestSuiteResponse>> GetByProjectIdAsync(long projectId, CancellationToken ct)
        {
            _logger?.LogDebug("ProjectCompositionService::GetByProjectIdAsync({ProjectId}) start", projectId);

            var projectComposition = await _context.ProjectTestSuiteCompositions
                .Where(pc => pc.ProjectId == projectId)
                .Include(_ => _.Project)
                .Include(_ => _.TestSuite)
                .AsNoTracking()
                .ToListAsync(ct);

            var testSuiteReponses = new List<GetTestSuiteResponse>();

            return testSuiteReponses;
        }

        /// <summary>
        /// Retrieves a single <see cref="ProjectTestSuiteComposition"/> by its identifier.
        /// Returns null when no matching entity is found.
        /// </summary>
        /// <param name="id">Identifier of the composition to retrieve.</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>The matching <see cref="ProjectTestSuiteComposition"/> or null if not found.</returns>
        public virtual async Task<ProjectTestSuiteComposition?> GetByIdAsync(long id, CancellationToken ct)
        {
            _logger?.LogDebug("ProjectCompositionService::GetByIdAsync({Id}) start", id);
            return await _context.ProjectTestSuiteCompositions
                .Where(pc => pc.Id == id)
                .Include(_ => _.Project)
                .Include(_ => _.TestSuite)
                .AsNoTracking()
                .FirstOrDefaultAsync(ct);
        }

        /// <summary>
        /// Creates a new <see cref="ProjectTestSuiteComposition"/> linking the specified project and test suite.
        /// Validates that both the project and the test suite exist and prevents creating duplicate links.
        /// Throws <see cref="InvalidOperationException"/> when validation fails.
        /// </summary>
        /// <param name="projectId">Identifier of the project to link.</param>
        /// <param name="testSuiteId">Identifier of the test suite to link.</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>The created <see cref="ProjectTestSuiteComposition"/>.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the project or test suite does not exist or the composition already exists.</exception>
        public virtual async Task<ProjectTestSuiteComposition> CreateAsync(long projectId, long testSuiteId, CancellationToken ct)
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
            var already = await _context.ProjectTestSuiteCompositions
                .AnyAsync(pc => pc.ProjectId == projectId && pc.TestSuiteId == testSuiteId, ct);
            if (already)
            {
                throw new InvalidOperationException("The composition already exists.");
            }

            var composition = new ProjectTestSuiteComposition
            {
                ProjectId = projectId,
                TestSuiteId = testSuiteId
            };

            _context.ProjectTestSuiteCompositions.Add(composition);

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
        /// Deletes a <see cref="ProjectTestSuiteComposition"/> by its identifier.
        /// </summary>
        /// <param name="id">Identifier of the composition to delete.</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>True if the composition was found and deleted; otherwise false.</returns>
        public virtual async Task<bool> DeleteAsync(long id, CancellationToken ct)
        {
            _logger?.LogDebug("ProjectCompositionService::DeleteAsync({Id}) start", id);

            var existing = await _context.ProjectTestSuiteCompositions
                .FirstOrDefaultAsync(pc => pc.Id == id, ct);
            if (existing == null)
            {
                return false;
            }

            _context.ProjectTestSuiteCompositions.Remove(existing);
            await _context.SaveChangesAsync(ct);
            return true;
        }
    }
}