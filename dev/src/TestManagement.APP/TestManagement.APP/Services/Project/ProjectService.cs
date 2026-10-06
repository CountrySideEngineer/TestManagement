using TestManagement.APP.ApiClients.Project;
using TestManagement.APP.Dto.Project.Get;
using TestManagement.APP.Dto.Project.Create;
using TestManagement.APP.ViewModel.Project;
using TestManagement.APP.ViewModel.TestSuite;
using TestManagement.APP.ViewModel.Tester;

namespace TestManagement.APP.Services.Project;

public class ProjectService : IProjectService
{
    private readonly ILogger<ProjectService> _logger;
    private readonly IProjectApiClient _apiClient;

    public ProjectService(
        ILogger<ProjectService> logger,
        IProjectApiClient apiClient)
    {
        _logger = logger;
        _apiClient = apiClient;
    }

    public async Task<ICollection<ProjectViewModel>> GetProjectsAsync()
    {
        _logger.LogInformation("ProjectService::GetProjectsAsync() start!");

        ICollection<GetProjectResponse> response = await _apiClient.GetProjectsAsync();

        return response
            .Select(project => new ProjectViewModel
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description
            })
            .ToList();
    }

    public async Task<ProjectViewModel?> GetProjectAsync(long id)
    {
        _logger.LogInformation("ProjectService::GetProjectAsync() start! Id: {Id}", id);

        var response = await _apiClient.GetProjectAsync(id);
        if (response is null)
        {
            return null;
        }

        return new ProjectViewModel
        {
            Id = response.Id,
            Name = response.Name,
            Description = response.Description
        };
    }

    public async Task<ICollection<TestSuiteViewModel>> GetProjectTestSuitesAsync(long id)
    {
        _logger.LogInformation("ProjectService::GetProjectTestSuitesAsync() start! Id: {Id}", id);

        var response = await _apiClient.GetProjectWithTestSuitesAsync(id);
        return response?.TestSuiteSummaries.Select(testSuite => new TestSuiteViewModel
        {
            Id = testSuite.Id,
            Name = testSuite.Name,
            Description = testSuite.Description
        }).ToList() ?? new List<TestSuiteViewModel>();
    }

    public async Task<ICollection<TesterViewModel>> GetProjectTestersAsync(long id)
    {
        _logger.LogInformation("ProjectService::GetProjectTestersAsync() start! Id: {Id}", id);

        var response = await _apiClient.GetProjectTestersAsync(id);
        return response.Select(tester => new TesterViewModel
        {
            Id = tester.Id,
            Name = tester.Name,
            Email = tester.Email
        }).ToList() ?? new List<TesterViewModel>();
    }

    public async Task<ProjectViewModel?> CreateProjectAsync(CreateProjectRequest request)
    {
        _logger.LogInformation("ProjectService::CreateProjectAsync() start! Name: {Name}", request.Name);

        var response = await _apiClient.CreateProjectAsync(request);
        if (response is null)
        {
            return null;
        }

        return new ProjectViewModel
        {
            Id = response.Id,
            Name = response.Name,
            Description = response.Description
        };
    }
}
