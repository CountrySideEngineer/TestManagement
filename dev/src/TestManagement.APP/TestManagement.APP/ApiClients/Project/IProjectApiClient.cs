using TestManagement.APP.Dto.Project.Get;
using TestManagement.APP.Dto.Project.Create;

namespace TestManagement.APP.ApiClients.Project;

public interface IProjectApiClient
{
    Task<ICollection<GetProjectResponse>> GetProjectsAsync();

    Task<GetProjectResponse?> GetProjectAsync(long id);

    Task<CreateProjectResponse?> CreateProjectAsync(CreateProjectRequest request);
}
