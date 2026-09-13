using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TestManagement.API.Features.Project.Get;
using TestManagement.API.Models;

namespace TestManagement.API.Services;

public class ProjectFacadeService : IProjectFacadeService
{
    private readonly IProjectService _projectService;
    private readonly IProjectCompositionService _projectCompositionService;
    private readonly ILogger<ProjectFacadeService> _logger;

    public ProjectFacadeService(
        IProjectService projectService,
        IProjectCompositionService projectCompositionService,
        ILogger<ProjectFacadeService> logger)
    {
        _projectService = projectService;
        _projectCompositionService = projectCompositionService;
        _logger = logger;
    }

    public Task<ICollection<GetProjectResponse>> GetAllAsync(CancellationToken ct = default)
    {
        _logger.LogDebug("ProjectFacadeService::GetAllAsync start");
        return _projectService.GetAllAsync(ct);
    }

    public Task<GetProjectResponse> GetByIdAsync(long id, CancellationToken ct = default)
    {
        _logger.LogDebug("ProjectFacadeService::GetByIdAsync start: {Id}", id);
        return _projectService.GetByIdAsync(id, ct);
    }

    public Task<ICollection<ProjectComposition>> GetCompositionsByProjectIdAsync(long projectId, CancellationToken ct = default)
    {
        _logger.LogDebug("ProjectFacadeService::GetCompositionsByProjectIdAsync start: {ProjectId}", projectId);
        return _projectCompositionService.GetByProjectIdAsync(projectId, ct);
    }

    public Task<ProjectComposition> CreateCompositionAsync(long projectId, long testSuiteId, CancellationToken ct = default)
    {
        _logger.LogDebug("ProjectFacadeService::CreateCompositionAsync start: {ProjectId} {TestSuiteId}", projectId, testSuiteId);
        return _projectCompositionService.CreateAsync(projectId, testSuiteId, ct);
    }

    public Task<bool> DeleteCompositionAsync(long id, CancellationToken ct = default)
    {
        _logger.LogDebug("ProjectFacadeService::DeleteCompositionAsync start: {Id}", id);
        return _projectCompositionService.DeleteAsync(id, ct);
    }
}
