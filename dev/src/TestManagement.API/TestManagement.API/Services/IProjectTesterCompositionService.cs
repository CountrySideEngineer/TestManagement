using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TestManagement.API.Models;

namespace TestManagement.API.Services;

/// <summary>
/// Interface for <see cref="ProjectTesterCompositionService"/>. Provides operations
/// to manage the join entity that associates projects and testers.
/// </summary>
public interface IProjectTesterCompositionService
{
    /// <summary>
    /// Returns compositions for a given project.
    /// </summary>
    Task<ICollection<ProjectTesterComposition>> GetByProjectIdAsync(long projectId, CancellationToken ct);

    /// <summary>
    /// Returns compositions for a given tester.
    /// </summary>
    Task<ICollection<ProjectTesterComposition>> GetByTesterIdAsync(long testerId, CancellationToken ct);

    /// <summary>
    /// Gets a composition by its identifier.
    /// </summary>
    Task<ProjectTesterComposition?> GetAsync(long id, CancellationToken ct);

    /// <summary>
    /// Creates a new composition between the specified project and tester.
    /// If an identical composition already exists, the existing one is returned.
    /// </summary>
    Task<ProjectTesterComposition> CreateCompositionAsync(long projectId, long testerId, CancellationToken ct);

    /// <summary>
    /// Deletes a composition by its identifier. Returns true if deleted.
    /// </summary>
    Task<bool> DeleteAsync(long id, CancellationToken ct);
}
