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

    public async Task<GetProjectWithTestSuiteResponse> GetByIdWithTestSuitesAsync(
        long projectId,
        CancellationToken ct = default
        )
    {
        _logger.LogDebug("ProjectFacadeService::GetByIdWithTestSuitesAsync start: {ProjectId}", projectId);

        GetProjectResponse projectResponse = await _projectService.GetByIdAsync(projectId, ct);
        var response = new GetProjectWithTestSuiteResponse
        {
            Id = projectId,
            Name = projectResponse.Name,
            Description = projectResponse.Description,
        };

        var composition = await _projectTestSuiteCompositionService.GetByProjectIdAsync(projectId, ct);
        if (composition is null)
        {
            response.TestSuiteSummaries = new();
            return response;
        }

        var summaries = composition
            .Select(cmp => new GetProjectWithTestSuiteResponse.TestSuiteSummary()
            {
                Id = cmp.TestSuite?.Id ?? 0,
                Name = cmp.TestSuite?.Name ?? string.Empty,
                Description = cmp.TestSuite?.Description ?? string.Empty,
            })
            .ToList();
        response.TestSuiteSummaries = summaries;

        return response;
    }

    public async Task<ICollection<GetTesterResponse>> GetTestersByProjectIdAsync(
        long projectId, 
        CancellationToken ct = default
        )
    {
        _logger.LogDebug("ProjectFacadeService::GetTestersByProjectIdAsync start: {ProjectId}", projectId);

        return null;
    }

    public async Task<GetTesterResponse> GetTesterByIdAsync(long testerId, CancellationToken ct = default)
    {
        _logger.LogDebug("ProjectFacadeService::GetTesterByIdAsync start: {TesterId}", testerId);

        return null;
    }

    public async Task<CreateProjectResponse> CreateProjectAsync(
        CreateProjectRequest request, 
        CancellationToken ct = default
        )
    {
        _logger.LogDebug("ProjectFacadeService::CreateProjectAsync start: {Request}", request);

        var response = await _projectService.CreateAsync(request, ct);

        return response;
    }

    public async Task<CreateProjectTestSuiteCompositionResponse> CreateTestSuiteCompositionAsync(
        CreateProjectTestSuiteCompositionRequest request,
        CancellationToken ct = default)
    {
        _logger.LogDebug("ProjectFacadeService::CreateTestSuiteCompositionAsync start: {ProjectId} {TestSuiteId}",
            request.ProjetId,
            request.TestSuiteId);

        return await _projectTestSuiteCompositionService.CreateAsync(request, ct);
    }

    public async Task<ICollection<CreateProjectTestSuiteCompositionResponse>> CreateTestSuiteCompositionAsync(
        ICollection<CreateProjectTestSuiteCompositionRequest> requests, 
        CancellationToken ct = default)
    {
        _logger.LogDebug("ProjectFacadeService::CreateTestSuiteCompositionAsync start.");

        return await _projectTestSuiteCompositionService.CreateAsync(requests, ct);
    }

    public async Task<bool> DeleteCompositionAsync(long id, CancellationToken ct = default)
    {
        _logger.LogDebug("ProjectFacadeService::DeleteCompositionAsync start: {Id}", id);

        return await _projectTestSuiteCompositionService.DeleteAsync(id, ct);
    }
}
