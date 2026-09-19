using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using TestManagement.API.Data;
using TestManagement.API.Features.Project.Create;
using TestManagement.API.Features.Project.Get;
using TestManagement.API.Models;

namespace TestManagement.API.Services;

/// <summary>
/// Service that provides operations for managing projects.
/// Uses the <see cref="TestManagementDbContext"/> to query project data
/// and maps entities to <see cref="GetProjectResponse"/> DTOs.
/// </summary>
public class ProjectService : IProjectService
{
    /// <summary>
    /// The EF Core database context used to access project entities.
    /// </summary>
    private readonly TestManagementDbContext _context;

    /// <summary>
    /// Optional logger for diagnostic messages from this service.
    /// </summary>
    private readonly ILogger<ProjectService>? _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectService"/> class.
    /// </summary>
    /// <param name="context">The database context to use for queries.</param>
    /// <param name="logger">Optional logger for diagnostic messages.</param>
    public ProjectService(
        TestManagementDbContext context,
        ILogger<ProjectService>? logger
        )
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves all projects from the database and maps them to DTOs.
    /// </summary>
    /// <param name="ct">A cancellation token to cancel the operation.</param>
    /// <returns>A collection of <see cref="GetProjectResponse"/> instances.</returns>
    public async Task<ICollection<GetProjectResponse>> GetAllAsync(CancellationToken ct = default)
    {
        _logger?.LogDebug("ProjectService::GetAllAsync() start!");

        return await _context.Projects
            .Select(p => new GetProjectResponse
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description
            })
            .ToListAsync(ct);
    }

    /// <summary>
    /// Retrieves a single project by its identifier and maps it to a DTO.
    /// </summary>
    /// <param name="id">The identifier of the project to retrieve.</param>
    /// <param name="ct">A cancellation token to cancel the operation.</param>
    /// <returns>
    /// A <see cref="GetProjectResponse"/> representing the project if found;
    /// otherwise an empty <see cref="GetProjectResponse"/> instance.
    /// </returns>
    public async Task<GetProjectResponse> GetByIdAsync(long id, CancellationToken ct = default)
    {
        _logger?.LogDebug("ProjectService::GetByIdAsync() start!");

        var project = await _context.Projects
            .Where(p => p.Id == id)
            .Select(p => new GetProjectResponse
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description
            })
            .FirstOrDefaultAsync(ct);

        if (project is null)
        {
            return new GetProjectResponse();
        }
        else
        {
            return project;
        }
    }

    public async Task<CreateProjectResponse> CreateAsync(CreateProjectRequest request, CancellationToken ct = default)
    {
        _logger?.LogDebug("ProjectService::CreateAsync() start!");

        var project = new Project
        {
            Name = request.Name,
            Description = request.Description
        };
        _context.Projects.Add(project);

        try
        {
            int result = await _context.SaveChangesAsync(ct);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error occurred while saving new project to the database.");
            throw;
        }

        var response = new CreateProjectResponse
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description
        };
        return response;
    }
}
