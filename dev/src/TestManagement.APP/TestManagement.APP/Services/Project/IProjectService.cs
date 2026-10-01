using TestManagement.APP.ViewModel.Project;
using TestManagement.APP.Dto.Project.Create;

namespace TestManagement.APP.Services.Project;

public interface IProjectService
{
    Task<ICollection<ProjectViewModel>> GetProjectsAsync();

    Task<ProjectViewModel?> CreateProjectAsync(CreateProjectRequest request);
}
