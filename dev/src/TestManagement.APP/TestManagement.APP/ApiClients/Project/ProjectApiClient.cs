using TestManagement.APP.Dto.Project.Get;
using TestManagement.APP.Dto.Project.Create;
using System.Net.Http.Json;

namespace TestManagement.APP.ApiClients.Project;

public class ProjectApiClient : IProjectApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ProjectApiClient> _logger;

    public ProjectApiClient(
        ILogger<ProjectApiClient> logger,
        IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient("TestApiClient");
    }

    public async Task<ICollection<GetProjectResponse>> GetProjectsAsync()
    {
        _logger.LogDebug("ProjectApiClient::GetProjectsAsync() start!");

        return await _httpClient.GetFromJsonAsync<List<GetProjectResponse>>("api/projects")
            ?? new List<GetProjectResponse>();
    }

    public async Task<GetProjectResponse?> GetProjectAsync(long id)
    {
        _logger.LogDebug("ProjectApiClient::GetProjectAsync() start! Id: {Id}", id);

        return await _httpClient.GetFromJsonAsync<GetProjectResponse>($"api/projects/{id}");
    }

    public async Task<CreateProjectResponse?> CreateProjectAsync(CreateProjectRequest request)
    {
        _logger.LogInformation("ProjectApiClient::CreateProjectAsync() start! Name: {Name}", request.Name);

        var response = await _httpClient.PostAsJsonAsync("api/projects", request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<CreateProjectResponse>();
    }
}
