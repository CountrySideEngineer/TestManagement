using Microsoft.EntityFrameworkCore;
using TestManagement.API.Data;
using TestManagement.API.Features.Tester.Create;
using TestManagement.API.Features.Tester.Get;
using TestManagement.API.Models;

namespace TestManagement.API.Services;

/// <summary>
/// Facade service that manages associations between <see cref="Project"/> and <see cref="Tester"/>.
/// Provides operations to list, assign, remove, and create-and-assign testers for a project.
/// This class is a thin convenience layer over the underlying composition data and the <see cref="TestManagementDbContext"/>.
/// </summary>
public class ProjectTesterFacadeService
{
    /// <summary>
    /// Database context used to access composition and related entities.
    /// </summary>
    private readonly TestManagementDbContext _dbContext;

    /// <summary>
    /// Optional logger instance for diagnostic and trace messages emitted by this service.
    /// </summary>
    private readonly ILogger<ProjectTesterFacadeService>? _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectTesterFacadeService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context used for data access.</param>
    /// <param name="logger">Optional logger instance for diagnostic messages.</param>
    public ProjectTesterFacadeService(TestManagementDbContext dbContext, ILogger<ProjectTesterFacadeService>? logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Returns all testers that are associated with the given project.
    /// </summary>
    /// <param name="projectId">Identifier of the project whose testers should be returned.</param>
    /// <param name="ct">Cancellation token for the async operation.</param>
    /// <returns>A collection of <see cref="GetTesterResponse"/> representing testers assigned to the project.</returns>
    public async Task<ICollection<GetTesterResponse>> GetTestersByProjectIdAsync(long projectId, CancellationToken ct)
    {
        _logger?.LogDebug("ProjectTesterFacadeService::GetTestersByProjectIdAsync({ProjectId}) start", projectId);

        var testers = await _dbContext.Set<ProjectTesterComposition>()
            .AsNoTracking()
            .Where(ptc => ptc.ProjectId == projectId)
            .Include(ptc => ptc.Tester)
            .Select(ptc => ptc.Tester!) // Tester is expected to be present for compositions
            .ToListAsync(ct);

        var response = testers
            .Select(t => new GetTesterResponse
            {
                Id = t.Id,
                Name = t.Name,
                Email = t.Email
            })
            .ToList();

        _logger?.LogDebug("ProjectTesterFacadeService::GetTestersByProjectIdAsync finished. Returning {Count} testers.", response.Count);

        return response;
    }

    /// <summary>
    /// Assigns an existing tester to a project.
    /// </summary>
    /// <param name="projectId">Identifier of the project to which the tester will be assigned.</param>
    /// <param name="testerId">Identifier of the tester to assign to the project.</param>
    /// <param name="ct">Cancellation token for the async operation.</param>
    /// <returns>True when a new association was created; false if the association already existed.</returns>
    public async Task<bool> AssignTesterToProjectAsync(long projectId, long testerId, CancellationToken ct)
    {
        _logger?.LogDebug("ProjectTesterFacadeService::AssignTesterToProjectAsync(ProjectId={ProjectId}, TesterId={TesterId}) start", projectId, testerId);

        var exists = await _dbContext.Set<ProjectTesterComposition>()
            .AnyAsync(ptc => ptc.ProjectId == projectId && ptc.TesterId == testerId, ct);

        if (exists)
        {
            _logger?.LogDebug("Association already exists.");
            return false;
        }

        var composition = new ProjectTesterComposition
        {
            ProjectId = projectId,
            TesterId = testerId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _dbContext.Set<ProjectTesterComposition>().Add(composition);

        try
        {
            await _dbContext.SaveChangesAsync(ct);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error occurred while saving project-tester association.");
            throw;
        }

        _logger?.LogDebug("Association created with Id={Id}", composition.Id);
        return true;
    }

    /// <summary>
    /// Removes the association between a tester and a project.
    /// </summary>
    /// <param name="projectId">Identifier of the project from which the tester will be removed.</param>
    /// <param name="testerId">Identifier of the tester to remove from the project.</param>
    /// <param name="ct">Cancellation token for the async operation.</param>
    /// <returns>True when the association was found and removed; false if no association existed.</returns>
    public async Task<bool> RemoveTesterFromProjectAsync(long projectId, long testerId, CancellationToken ct)
    {
        _logger?.LogDebug("ProjectTesterFacadeService::RemoveTesterFromProjectAsync(ProjectId={ProjectId}, TesterId={TesterId}) start", projectId, testerId);

        var composition = await _dbContext.Set<ProjectTesterComposition>()
            .Where(ptc => ptc.ProjectId == projectId && ptc.TesterId == testerId)
            .FirstOrDefaultAsync(ct);

        if (composition is null)
        {
            _logger?.LogDebug("Association not found.");
            return false;
        }

        _dbContext.Set<ProjectTesterComposition>().Remove(composition);

        try
        {
            await _dbContext.SaveChangesAsync(ct);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error occurred while removing project-tester association.");
            throw;
        }

        _logger?.LogDebug("Association removed (Id={Id}).", composition.Id);
        return true;
    }

    /// <summary>
    /// Creates a new tester and assigns it to the given project in a single operation.
    /// </summary>
    /// <param name="request">The create request containing tester details.</param>
    /// <param name="projectId">Identifier of the project to which the created tester will be assigned.</param>
    /// <param name="ct">Cancellation token for the async operation.</param>
    /// <returns>A <see cref="CreateTesterResponse"/> containing the created tester's details.</returns>
    public async Task<CreateTesterResponse> CreateAndAssignTesterAsync(CreateTesterRequest request, long projectId, CancellationToken ct)
    {
        _logger?.LogDebug("ProjectTesterFacadeService::CreateAndAssignTesterAsync(ProjectId={ProjectId}) start", projectId);

        var tester = new Tester
        {
            Name = request.Name,
            Email = request.Email,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var composition = new ProjectTesterComposition
        {
            ProjectId = projectId,
            Tester = tester,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _dbContext.Set<ProjectTesterComposition>().Add(composition);

        try
        {
            await _dbContext.SaveChangesAsync(ct);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error occurred while creating tester and association.");
            throw;
        }

        var response = new CreateTesterResponse
        {
            Id = tester.Id,
            Name = tester.Name,
            Email = tester.Email
        };

        _logger?.LogDebug("ProjectTesterFacadeService::CreateAndAssignTesterAsync finished. Created tester Id={Id}", tester.Id);

        return response;
    }
}
