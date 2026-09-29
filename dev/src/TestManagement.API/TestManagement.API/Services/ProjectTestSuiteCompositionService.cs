using Microsoft.EntityFrameworkCore;
using TestManagement.API.Data;
using TestManagement.API.Features.Project.Create;
using TestManagement.API.Features.Project.Get;
using TestManagement.API.Features.TestSuite.Get;
using TestManagement.API.Models;

namespace TestManagement.API.Services
{
    /// <summary>
    /// Service that provides operations to manage project and test suite compositions.
    /// </summary>
    public class ProjectTestSuiteCompositionService : IProjectTestSuiteCompositionService
    {
        /// <summary>Database context used to query and persist compositions.</summary>
        private readonly TestManagementDbContext _context;
        /// <summary>Optional logger used for service diagnostics.</summary>
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
        public virtual async Task<ICollection<ProjectTestSuiteComposition>?> GetByProjectIdAsync(long projectId, CancellationToken ct)
        {
            _logger?.LogDebug("ProjectCompositionService::GetByProjectIdAsync({ProjectId}) start", projectId);

            var projectComposition = await _context.ProjectTestSuiteCompositions
                .Where(pc => pc.ProjectId == projectId)
                .Include(_ => _.Project)
                .Include(_ => _.TestSuite)
                .AsNoTracking()
                .ToListAsync(ct);

            return projectComposition;
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
        /// Creates a new ProjectTestSuiteComposition that links a project and a test suite.
        /// </summary>
        /// <param name="request">Request containing the project and test suite identifiers.</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>The created <see cref="ProjectTestSuiteComposition"/> entity.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the referenced project or test suite does not exist, or when the composition already exists.
        /// </exception>
        /// <exception cref="DbUpdateException">
        /// Thrown when saving changes to the database fails.
        /// </exception>
        public virtual async Task<CreateProjectTestSuiteCompositionResponse> CreateAsync(
            CreateProjectTestSuiteCompositionRequest request,
            CancellationToken ct
            )
        {
            _logger?.LogDebug("ProjectCompositionService::CreateAsync(projectId={ProjectId}, testSuiteId={TestSuiteId}) start",
                request.ProjetId,
                request.TestSuiteId);

            var response = await RegisterCompositionItemAsync(request, ct);

            try
            {
                await _context.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex)
            {
                // Log the exception with context and rethrow to let upstream handle it.
                _logger?.LogError(ex, "Failed to create ProjectComposition(projectId={ProjectId}, testSuiteId={TestSuiteId})",
                    response.ProjectId,
                    response.TestSuiteId);
                throw;
            }

            // Return the newly created composition entity.
            return response;
        }

        /// <summary>
        /// Creates multiple project and test suite compositions and saves them as one operation.
        /// </summary>
        /// <param name="requests">Requests containing the project and test suite identifiers.</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>The responses for the created compositions.</returns>
        public async Task<ICollection<CreateProjectTestSuiteCompositionResponse>> CreateAsync(ICollection<CreateProjectTestSuiteCompositionRequest> requests, CancellationToken ct)
        {
            _logger?.LogDebug("ProjectCompositionService::CreateAsync(request count = {0}) start", requests.Count);

            var responses = new List<CreateProjectTestSuiteCompositionResponse>();
            foreach (var request in requests)
            {
                var response = await RegisterCompositionItemAsync(request, ct);
                responses.Add(response);
            }

            try
            {
                await _context.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex)
            {
                // Log the exception with context and rethrow to let upstream handle it.
                _logger?.LogError(ex, "Failed to create ProjectCompositions.");
                throw;
            }

            return responses;
        }

        /// <summary>
        /// Validates a composition request, registers the composition with the database context,
        /// and creates its response without saving changes.
        /// </summary>
        /// <param name="request">Request containing the project and test suite identifiers.</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>The response for the registered composition.</returns>
        protected async Task<CreateProjectTestSuiteCompositionResponse> RegisterCompositionItemAsync(CreateProjectTestSuiteCompositionRequest request, CancellationToken ct)
        {
            _logger?.LogDebug("ProjectCompositionService::RegisterCompositionItemAsync(projectId={ProjectId}, testSuiteId={TestSuiteId}) start",
                request.ProjetId,
                request.TestSuiteId);

            var projectExists = await _context.Projects.AnyAsync(p => p.Id == request.ProjetId, ct);
            if (!projectExists)
            {
                throw new InvalidOperationException($"Project with id {request.ProjetId} does not exist.");
            }

            // Check that the test suite exists before creating the composition.
            var testSuiteExists = await _context.TestSuites.AnyAsync(ts => ts.Id == request.TestSuiteId, ct);
            if (!testSuiteExists)
            {
                throw new InvalidOperationException($"TestSuite with id {request.TestSuiteId} does not exist.");
            }

            // Prevent duplicate (unique index exists at DB level) ---------------------------
            // Verify there is no existing composition for the same project and test suite.
            var already = await _context.ProjectTestSuiteCompositions
                .AnyAsync(pc => pc.ProjectId == request.ProjetId && pc.TestSuiteId == request.TestSuiteId, ct);
            if (already)
            {
                throw new InvalidOperationException("The composition already exists.");
            }

            // Create the composition entity and attach it to the context --------------------
            var composition = new ProjectTestSuiteComposition
            {
                ProjectId = request.ProjetId,
                TestSuiteId = request.TestSuiteId
            };

            _context.ProjectTestSuiteCompositions.Add(composition);

            var response = new CreateProjectTestSuiteCompositionResponse
            {
                CompositionId = composition.Id,
                ProjectId = composition.ProjectId,
                TestSuiteId = composition.TestSuiteId
            };

            return response;
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