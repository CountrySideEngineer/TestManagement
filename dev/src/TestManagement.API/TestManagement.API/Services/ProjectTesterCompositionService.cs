using Microsoft.EntityFrameworkCore;
using TestManagement.API.Data;
using TestManagement.API.Models;

namespace TestManagement.API.Services;

/// <summary>
/// Service to manage Project-Tester composition entities.
/// Encapsulates common operations against the ProjectTesterComposition join entity.
/// </summary>
public class ProjectTesterCompositionService : IProjectTesterCompositionService
{
    /// <summary>
    /// Database context used to access ProjectTesterComposition entities.
    /// </summary>
    private readonly TestManagementDbContext _dbContext;

    /// <summary>
    /// Optional logger for diagnostic messages from this service.
    /// </summary>
    private readonly ILogger<ProjectTesterCompositionService>? _logger;

    /// <summary>
    /// Initializes a new instance of <see cref="ProjectTesterCompositionService"/>.
    /// </summary>
    /// <param name="dbContext">The database context used for data access.</param>
    /// <param name="logger">Optional logger instance.</param>
    public ProjectTesterCompositionService(
        TestManagementDbContext dbContext, 
        ILogger<ProjectTesterCompositionService>? logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Returns compositions for a given project.
    /// </summary>
    /// <param name="projectId">Identifier of the project to filter compositions.</param>
    /// <param name="ct">Cancellation token to cancel the operation.</param>
    /// <returns>A collection of <see cref="ProjectTesterComposition"/> instances associated with the project.</returns>
    public async Task<ICollection<ProjectTesterComposition>> GetByProjectIdAsync(
        long projectId, 
        CancellationToken ct)
    {
        _logger?.LogDebug("ProjectTesterCompositionService::GetByProjectIdAsync({ProjectId}) start", projectId);

        var compositions = await _dbContext.Set<ProjectTesterComposition>()
            .AsNoTracking()
            .Where(c => c.ProjectId == projectId)
            .Include(c => c.Tester)
            .ToListAsync(ct);

        _logger?.LogDebug("ProjectTesterCompositionService::GetByProjectIdAsync finished. Returning {Count} compositions.", compositions.Count);

        return compositions;
    }

    /// <summary>
    /// Returns compositions for a given tester.
    /// </summary>
    /// <param name="testerId">Identifier of the tester to filter compositions.</param>
    /// <param name="ct">Cancellation token to cancel the operation.</param>
    /// <returns>A collection of <see cref="ProjectTesterComposition"/> instances associated with the tester.</returns>
    public async Task<ICollection<ProjectTesterComposition>> GetByTesterIdAsync(
        long testerId, 
        CancellationToken ct)
    {
        _logger?.LogDebug("ProjectTesterCompositionService::GetByTesterIdAsync({TesterId}) start", testerId);

        var compositions = await _dbContext.Set<ProjectTesterComposition>()
            .AsNoTracking()
            .Where(c => c.TesterId == testerId)
            .Include(c => c.Project)
            .ToListAsync(ct);

        _logger?.LogDebug("ProjectTesterCompositionService::GetByTesterIdAsync finished. Returning {Count} compositions.", compositions.Count);

        return compositions;
    }

    /// <summary>
    /// Gets a composition by its identifier.
    /// </summary>
    /// <param name="id">Identifier of the composition.</param>
    /// <param name="ct">Cancellation token to cancel the operation.</param>
    /// <returns>The matching <see cref="ProjectTesterComposition"/> if found; otherwise null.</returns>
    public async Task<ProjectTesterComposition?> GetAsync(long id, CancellationToken ct)
    {
        _logger?.LogDebug("ProjectTesterCompositionService::GetAsync({Id}) start", id);

        var composition = await _dbContext.Set<ProjectTesterComposition>()
            .AsNoTracking()
            .Include(c => c.Project)
            .Include(c => c.Tester)
            .FirstOrDefaultAsync(c => c.Id == id, ct);

        return composition;
    }

    /// <summary>
    /// Creates a new composition between the specified project and tester.
    /// If an identical composition already exists, the existing one is returned.
    /// </summary>
    /// <param name="projectId">Identifier of the project to associate.</param>
    /// <param name="testerId">Identifier of the tester to associate.</param>
    /// <param name="ct">Cancellation token to cancel the operation.</param>
    /// <returns>The created or existing <see cref="ProjectTesterComposition"/> instance.</returns>
    /// <exception cref="Exception">Propagates exceptions thrown while saving to the database.</exception>
    public async Task<ProjectTesterComposition> CreateCompositionAsync(
        long projectId, 
        long testerId, 
        CancellationToken ct)
    {
        _logger?.LogDebug("ProjectTesterCompositionService::CreateCompositionAsync(ProjectId={ProjectId}, TesterId={TesterId}) start",
            projectId,
            testerId);

        var compositions = _dbContext.ProjectTesterCompositions;

        var existing = await compositions.FirstOrDefaultAsync(c => c.ProjectId == projectId && c.TesterId == testerId, ct);
        if (existing is not null)
        {
            _logger?.LogDebug("Composition already exists with Id={Id}", existing.Id);
            return existing;
        }

        var composition = new ProjectTesterComposition
        {
            ProjectId = projectId,
            TesterId = testerId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        compositions.Add(composition);

        try
        {
            await _dbContext.SaveChangesAsync(ct);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error occurred while creating ProjectTesterComposition.");
            throw;
        }

        _logger?.LogDebug("ProjectTesterCompositionService::CreateAsync finished. Created Id={Id}", composition.Id);

        return composition;
    }

    /// <summary>
    /// Deletes a composition by its identifier. Returns true if deleted.
    /// </summary>
    /// <param name="id">Identifier of the composition to delete.</param>
    /// <param name="ct">Cancellation token to cancel the operation.</param>
    /// <returns>True when the composition was found and deleted; false if not found.</returns>
    /// <exception cref="Exception">Propagates exceptions thrown while saving to the database.</exception>
    public async Task<bool> DeleteAsync(long id, CancellationToken ct)
    {
        _logger?.LogDebug("ProjectTesterCompositionService::DeleteAsync({Id}) start", id);

        var set = _dbContext.Set<ProjectTesterComposition>();
        var composition = await set.FirstOrDefaultAsync(c => c.Id == id, ct);
        if (composition is null)
        {
            _logger?.LogDebug("Composition not found.");
            return false;
        }

        set.Remove(composition);

        try
        {
            await _dbContext.SaveChangesAsync(ct);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error occurred while deleting ProjectTesterComposition.");
            throw;
        }

        _logger?.LogDebug("ProjectTesterCompositionService::DeleteAsync finished. Deleted Id={Id}", id);
        return true;
    }
}
