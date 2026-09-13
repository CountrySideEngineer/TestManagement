using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TestManagement.API.Features.Project.Get;
using TestManagement.API.Models;

namespace TestManagement.API.Services;

/// <summary>
/// Facade that coordinates project and project-composition operations for application-level use.
/// </summary>
public interface IProjectFacadeService
{
    Task<ICollection<GetProjectResponse>> GetAllAsync(CancellationToken ct = default);

    Task<GetProjectResponse> GetByIdAsync(long id, CancellationToken ct = default);

    Task<ICollection<ProjectComposition>> GetCompositionsByProjectIdAsync(long projectId, CancellationToken ct = default);

    Task<ProjectComposition> CreateCompositionAsync(long projectId, long testSuiteId, CancellationToken ct = default);

    Task<bool> DeleteCompositionAsync(long id, CancellationToken ct = default);
}
