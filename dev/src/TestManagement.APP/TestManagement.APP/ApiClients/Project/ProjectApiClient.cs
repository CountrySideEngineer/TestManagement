using System.Net.Http.Json;
using TestManagement.APP.Dto.Project.Get;

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
}
