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

/// <summary>
/// Coordinates project operations and project composition use cases.
/// </summary>
public class ProjectFacadeService : IProjectFacadeService
{
    /// <summary>Service that manages projects.</summary>
    private readonly IProjectService _projectService;
    /// <summary>Service that manages project and test suite compositions.</summary>
    private readonly IProjectTestSuiteCompositionService _projectTestSuiteCompositionService;
    /// <summary>Service that manages project and tester compositions.</summary>
    private readonly IProjectTesterCompositionService _projectTesterCompositionService;
    /// <summary>Logger used for facade diagnostics.</summary>
    private readonly ILogger<ProjectFacadeService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectFacadeService"/> class.
    /// </summary>
    /// <param name="projectService">Service that manages projects.</param>
    /// <param name="projectCompositionService">Service that manages project and test suite compositions.</param>
    /// <param name="projectTesterCompositionService">Service that manages project and tester compositions.</param>
    /// <param name="logger">Logger used for diagnostics.</param>
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

    /// <summary>Retrieves all projects.</summary>
    /// <param name="ct">Cancellation token to cancel the operation.</param>
    /// <returns>All projects.</returns>
    public async Task<ICollection<GetProjectResponse>> GetAllAsync(CancellationToken ct = default)
    {
        _logger.LogDebug("ProjectFacadeService::GetAllAsync start");

        return await _projectService.GetAllAsync(ct);
    }

    /// <summary>Retrieves a project by its identifier.</summary>
    /// <param name="id">Identifier of the project.</param>
    /// <param name="ct">Cancellation token to cancel the operation.</param>
    /// <returns>The requested project.</returns>
    public async Task<GetProjectResponse> GetByIdAsync(long id, CancellationToken ct = default)
    {
        _logger.LogDebug("ProjectFacadeService::GetByIdAsync start: {Id}", id);

        return await _projectService.GetByIdAsync(id, ct);
    }

    /// <summary>Retrieves a project together with its test suite summaries.</summary>
    /// <param name="projectId">Identifier of the project.</param>
    /// <param name="ct">Cancellation token to cancel the operation.</param>
    /// <returns>The project and its associated test suites.</returns>
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

    /// <summary>Retrieves testers associated with a project.</summary>
    /// <param name="projectId">Identifier of the project.</param>
    /// <param name="ct">Cancellation token to cancel the operation.</param>
    /// <returns>The project's testers.</returns>
    public async Task<ICollection<GetTesterResponse>> GetTestersByProjectIdAsync(
        long projectId, 
        CancellationToken ct = default
        )
    {
        _logger.LogDebug("ProjectFacadeService::GetTestersByProjectIdAsync start: {ProjectId}", projectId);

        return null;
    }

    /// <summary>Retrieves a tester by its identifier.</summary>
    /// <param name="testerId">Identifier of the tester.</param>
    /// <param name="ct">Cancellation token to cancel the operation.</param>
    /// <returns>The requested tester.</returns>
    public async Task<GetTesterResponse> GetTesterByIdAsync(long testerId, CancellationToken ct = default)
    {
        _logger.LogDebug("ProjectFacadeService::GetTesterByIdAsync start: {TesterId}", testerId);

        return null;
    }

    /// <summary>Creates a new project.</summary>
    /// <param name="request">Request containing the project data.</param>
    /// <param name="ct">Cancellation token to cancel the operation.</param>
    /// <returns>The created project.</returns>
    public async Task<CreateProjectResponse> CreateProjectAsync(
        CreateProjectRequest request, 
        CancellationToken ct = default
        )
    {
        _logger.LogDebug("ProjectFacadeService::CreateProjectAsync start: {Request}", request);

        var response = await _projectService.CreateAsync(request, ct);

        return response;
    }

    /// <summary>Creates a project and test suite composition.</summary>
    /// <param name="request">Request containing the project and test suite identifiers.</param>
    /// <param name="ct">Cancellation token to cancel the operation.</param>
    /// <returns>The created composition.</returns>
    public async Task<CreateProjectTestSuiteCompositionResponse> CreateTestSuiteCompositionAsync(
        CreateProjectTestSuiteCompositionRequest request,
        CancellationToken ct = default)
    {
        _logger.LogDebug("ProjectFacadeService::CreateTestSuiteCompositionAsync start: {ProjectId} {TestSuiteId}",
            request.ProjetId,
            request.TestSuiteId);

        return await _projectTestSuiteCompositionService.CreateAsync(request, ct);
    }

    /// <summary>Creates multiple project and test suite compositions.</summary>
    /// <param name="requests">Requests containing the project and test suite identifiers.</param>
    /// <param name="ct">Cancellation token to cancel the operation.</param>
    /// <returns>The created compositions.</returns>
    public async Task<ICollection<CreateProjectTestSuiteCompositionResponse>> CreateTestSuiteCompositionAsync(
        ICollection<CreateProjectTestSuiteCompositionRequest> requests, 
        CancellationToken ct = default)
    {
        _logger.LogDebug("ProjectFacadeService::CreateTestSuiteCompositionAsync start.");

        return await _projectTestSuiteCompositionService.CreateAsync(requests, ct);
    }

    /// <summary>Deletes a project composition by its identifier.</summary>
    /// <param name="id">Identifier of the composition.</param>
    /// <param name="ct">Cancellation token to cancel the operation.</param>
    /// <returns>True when the composition was deleted; otherwise false.</returns>
    public async Task<bool> DeleteCompositionAsync(long id, CancellationToken ct = default)
    {
        _logger.LogDebug("ProjectFacadeService::DeleteCompositionAsync start: {Id}", id);

        return await _projectTestSuiteCompositionService.DeleteAsync(id, ct);
    }
}
