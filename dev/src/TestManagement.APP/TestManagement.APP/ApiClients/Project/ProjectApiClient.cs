using TestManagement.APP.Dto.Project.Get;
using TestManagement.APP.Dto.Project.Create;
using System.Net.Http.Json;

namespace TestManagement.APP.ApiClients.Project;

/// <summary>
/// HTTP client for project-related API operations.
/// </summary>
public class ProjectApiClient : IProjectApiClient
{
    /// <summary>
    /// HTTP client configured for the test management API.
    /// </summary>
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Logger used to record API client diagnostics.
    /// </summary>
    private readonly ILogger<ProjectApiClient> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectApiClient"/> class.
    /// </summary>
    /// <param name="logger">Logger used for diagnostics.</param>
    /// <param name="httpClientFactory">Factory used to create the configured HTTP client.</param>
    public ProjectApiClient(
        ILogger<ProjectApiClient> logger,
        IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient("TestApiClient");
    }

    /// <summary>
    /// Retrieves all projects from the API.
    /// </summary>
    /// <returns>A collection of projects.</returns>
    public async Task<ICollection<GetProjectResponse>> GetProjectsAsync()
    {
        _logger.LogDebug("ProjectApiClient::GetProjectsAsync() start!");

        return await _httpClient.GetFromJsonAsync<List<GetProjectResponse>>("api/projects")
            ?? new List<GetProjectResponse>();
    }

    /// <summary>
    /// Retrieves a project by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the project to retrieve.</param>
    /// <returns>The project response, or <see langword="null"/> when the response has no content.</returns>
    public async Task<GetProjectResponse?> GetProjectAsync(long id)
    {
        _logger.LogDebug("ProjectApiClient::GetProjectAsync() start! Id: {Id}", id);

        return await _httpClient.GetFromJsonAsync<GetProjectResponse>($"api/projects/{id}");
    }

    /// <summary>
    /// Creates a project through the API.
    /// </summary>
    /// <param name="request">The project creation request.</param>
    /// <returns>The created project response, or <see langword="null"/> when the response has no content.</returns>
    public async Task<CreateProjectResponse?> CreateProjectAsync(CreateProjectRequest request)
    {
        _logger.LogInformation("ProjectApiClient::CreateProjectAsync() start! Name: {Name}", request.Name);

        var response = await _httpClient.PostAsJsonAsync("api/projects", request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<CreateProjectResponse>();
    }
}
