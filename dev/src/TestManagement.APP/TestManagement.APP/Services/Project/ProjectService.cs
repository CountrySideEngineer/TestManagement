using TestManagement.APP.ApiClients.Project;
using TestManagement.APP.Dto.Project.Get;
using TestManagement.APP.Dto.Project.Create;
using TestManagement.APP.ViewModel.Project;

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
