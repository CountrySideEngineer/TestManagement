using TestManagement.APP.ViewModel.Project;
using TestManagement.APP.Dto.Project.Create;
using TestManagement.APP.ViewModel.TestSuite;
using TestManagement.APP.ViewModel.Tester;

namespace TestManagement.APP.Services.Project;

public interface IProjectService
{
    Task<ICollection<ProjectViewModel>> GetProjectsAsync();

    Task<ProjectViewModel?> GetProjectAsync(long id);

    Task<ICollection<TestSuiteViewModel>> GetProjectTestSuitesAsync(long id);

    Task<ICollection<TesterViewModel>> GetProjectTestersAsync(long id);

    Task<ProjectViewModel?> CreateProjectAsync(CreateProjectRequest request);
}
