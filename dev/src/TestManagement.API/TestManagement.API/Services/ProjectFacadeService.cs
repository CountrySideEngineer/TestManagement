using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TestManagement.API.Features.Project.Get;
using TestManagement.API.Features.Tester.Get;
using TestManagement.API.Services;
using TestManagement.API.Models;

using GetProjectResponse = TestManagement.API.Features.Project.Get.GetProjectResponse;
using TestManagement.API.Features.Project.Create;

namespace TestManagement.API.Services;

public class ProjectFacadeService : IProjectFacadeService
{
    private readonly IProjectService _projectService;
    private readonly IProjectTestSuiteCompositionService _projectTestSuiteCompositionService;
    private readonly IProjectTesterCompositionService _projectTesterCompositionService;
    private readonly ILogger<ProjectFacadeService> _logger;

    public ProjectFacadeService(
        IProjectService projectService,
        IProjectTestSuiteCompositionService projectCompositionService,
        IProjectTesterCompositionService projectTesterCompositionService,
        ILogger<ProjectFacadeService> logger)
    {
        _projectService = projectService;
        _projectTestSuiteCompositionService = projectCompositionService;
        _projectTesterCompositionService = projectTesterCompositionService;
        _logger = logger;
    }

    public async Task<ICollection<GetProjectResponse>> GetAllAsync(CancellationToken ct = default)
    {
        _logger.LogDebug("ProjectFacadeService::GetAllAsync start");

        return await _projectService.GetAllAsync(ct);
    }

    public async Task<GetProjectResponse> GetByIdAsync(long id, CancellationToken ct = default)
    {
        _logger.LogDebug("ProjectFacadeService::GetByIdAsync start: {Id}", id);

        return await _projectService.GetByIdAsync(id, ct);
    }

    public async Task<ICollection<Models.ProjectTestSuiteComposition>> GetCompositionsByProjectIdAsync(long projectId, CancellationToken ct = default)
    {
        _logger.LogDebug("ProjectFacadeService::GetCompositionsByProjectIdAsync start: {ProjectId}", projectId);

        GetProjectResponse projectResponse = await _projectService.GetByIdAsync(projectId, ct);

        var projectTestSuiteCompositions = await _projectTestSuiteCompositionService.GetByProjectIdAsync(projectId, ct);

        projectResponse.TestSuites = projectTestSuiteCompositions.ToList();

        return null;
    }

    public async Task<ICollection<GetTesterResponse>> GetTestersByProjectIdAsync(long projectId, CancellationToken ct = default)
    {
        _logger.LogDebug("ProjectFacadeService::GetTestersByProjectIdAsync start: {ProjectId}", projectId);

        return null;
    }

    public async Task<GetTesterResponse> GetTesterByIdAsync(long testerId, CancellationToken ct = default)
    {
        _logger.LogDebug("ProjectFacadeService::GetTesterByIdAsync start: {TesterId}", testerId);

        return null;
    }

    public async Task<CreateProjectResponse> CreateProjectAsync(CreateProjectRequest request, CancellationToken ct = default)
    {
        _logger.LogDebug("ProjectFacadeService::CreateProjectAsync start: {Request}", request);

        var response = await _projectService.CreateAsync(request, ct);

        return response;
    }

    public async Task<Models.ProjectTestSuiteComposition> CreateTestSuiteCompositionAsync(
        CreateProjectTestSuiteCompositionRequest request,
        CancellationToken ct = default)
    {
        _logger.LogDebug("ProjectFacadeService::CreateTestSuiteCompositionAsync start: {ProjectId} {TestSuiteId}",
            request.ProjetId,
            request.TestSuiteId);

        return await _projectTestSuiteCompositionService.CreateAsync(request, ct);
    }

    public async Task<bool> DeleteCompositionAsync(long id, CancellationToken ct = default)
    {
        _logger.LogDebug("ProjectFacadeService::DeleteCompositionAsync start: {Id}", id);

        return await _projectTestSuiteCompositionService.DeleteAsync(id, ct);
    }
}
