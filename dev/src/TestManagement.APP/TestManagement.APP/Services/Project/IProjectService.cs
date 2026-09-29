using TestManagement.APP.ViewModel.Project;

namespace TestManagement.APP.Services.Project;

public interface IProjectService
{
    Task<ICollection<ProjectViewModel>> GetProjectsAsync();
}
