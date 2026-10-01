using TestManagement.APP.Dto.Project.Get;
using TestManagement.APP.Dto.Project.Create;

namespace TestManagement.APP.ApiClients.Project;

/// <summary>
/// Defines API operations for retrieving and creating projects.
/// </summary>
public interface IProjectApiClient
{
    /// <summary>
    /// Retrieves all projects.
    /// </summary>
    /// <returns>A collection of project responses.</returns>
    Task<ICollection<GetProjectResponse>> GetProjectsAsync();

    /// <summary>
    /// Retrieves a project by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the project to retrieve.</param>
    /// <returns>The project response, or <see langword="null"/> when no response content is available.</returns>
    Task<GetProjectResponse?> GetProjectAsync(long id);

    /// <summary>
    /// Creates a project.
    /// </summary>
    /// <param name="request">The project creation request.</param>
    /// <returns>The created project response, or <see langword="null"/> when no response content is available.</returns>
    Task<CreateProjectResponse?> CreateProjectAsync(CreateProjectRequest request);
}
