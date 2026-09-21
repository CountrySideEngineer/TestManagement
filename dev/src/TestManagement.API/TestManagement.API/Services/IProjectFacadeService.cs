using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TestManagement.API.Features.Project.Get;
using TestManagement.API.Models;
using TestManagement.API.Features.Tester.Get;
using TestManagement.API.Features.Project.Create;

namespace TestManagement.API.Services;

/// <summary>
/// Facade that coordinates project and project-composition operations for application-level use.
/// </summary>
public interface IProjectFacadeService
{
    Task<ICollection<Features.Project.Get.GetProjectResponse>> GetAllAsync(CancellationToken ct = default);

    Task<Features.Project.Get.GetProjectResponse> GetByIdAsync(long id, CancellationToken ct = default);

    Task<ICollection<Models.ProjectTestSuiteComposition>> GetCompositionsByProjectIdAsync(long projectId, CancellationToken ct = default);

    Task<CreateProjectResponse> CreateProjectAsync(CreateProjectRequest request, CancellationToken ct = default);

    Task<Models.ProjectTestSuiteComposition> CreateTestSuiteCompositionAsync(
        CreateProjectTestSuiteCompositionRequest request,
        CancellationToken ct = default);

    Task<bool> DeleteCompositionAsync(long id, CancellationToken ct = default);

    /// <summary>
    /// Returns the testers that are assigned to a project.
    /// </summary>
    Task<ICollection<GetTesterResponse>> GetTestersByProjectIdAsync(long projectId, CancellationToken ct = default);

    /// <summary>
    /// Retrieves a tester by its identifier.
    /// </summary>
    Task<GetTesterResponse> GetTesterByIdAsync(long testerId, CancellationToken ct = default);
}
