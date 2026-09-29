using TestManagement.APP.Dto.Project.Get;

namespace TestManagement.APP.ApiClients.Project;

public interface IProjectApiClient
{
    Task<ICollection<GetProjectResponse>> GetProjectsAsync();
}
