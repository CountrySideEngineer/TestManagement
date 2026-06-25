using System.Runtime.CompilerServices;
using TestManagement.APP.Dto.Environment.Get;
using TestManagement.APP.Dto.Environment.Post;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;

namespace TestManagement.APP.ApiClients.Environment
{
    /// <summary>
    /// API client responsible for environment-related HTTP calls to the Test API.
    /// Implements <see cref="IEnvironmentApiClient"/> and performs JSON serialization.
    /// </summary>
    public class EnvironmentApiClient : IEnvironmentApiClient
    {
        /// <summary>
        /// Logger used for diagnostic messages from this client.
        /// </summary>
        private readonly ILogger<EnvironmentApiClient> _logger;

        /// <summary>
        /// HttpClient instance configured for the Test API (named client "TestApiClient").
        /// </summary>
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Creates a new <see cref="EnvironmentApiClient"/>.
        /// </summary>
        /// <param name="logger">Logger instance provided by DI.</param>
        /// <param name="httpClientFactory">Factory used to create an <see cref="HttpClient"/> for the API.</param>
        public EnvironmentApiClient(
            ILogger<EnvironmentApiClient> logger,
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient("TestApiClient");
        }

        /// <summary>
        /// Retrieves environments that match the provided name from the Test API.
        /// </summary>
        /// <param name="request">A <see cref="GetEnvironmentRequest"/> containing the name to search for.</param>
        /// <returns>A list of <see cref="GetEnvironmentResponse"/> objects. Returns an empty list if no matches are found.</returns>
        public async Task<IList<GetEnvironmentResponse>> GetEnvironmentsByNameAsync(GetEnvironmentRequest request)
        {
            _logger.LogInformation("EnvironmentApiClient::GetEnvironmentByNameAsync() start! Name: {Request.Name}", request.Name);

            string apiUrl = $"api/environment/name/{request.Name}";
            var responses = await _httpClient!
                .GetFromJsonAsync<List<GetEnvironmentResponse>>(apiUrl) ??
                    new List<GetEnvironmentResponse>();

            return responses;
        }

        /// <summary>
        /// Retrieves the list of environments from the remote API.
        /// Returns an empty list if the API returns no data.
        /// </summary>
        /// <returns>A list of <see cref="GetEnvironmentResponse"/> objects.</returns>
        public async Task<IList<GetEnvironmentResponse>> GetEnvironmentsAsync()
        {
            _logger?.LogDebug("EnvironmentApiClient::GetEnvironmentsAsync() start!");

            var result = await _httpClient!
                .GetFromJsonAsync<List<GetEnvironmentResponse>>("api/environment") ?? 
                    new List<GetEnvironmentResponse>();

            return result;
        }

        /// <summary>
        /// Creates a new environment by posting the specified <see cref="PostEnvironmentRequest"/> to the Test API.
        /// </summary>
        /// <param name="request">The environment data to create.</param>
        /// <returns>
        /// A <see cref="PostEnvironmentResponse"/> representing the created environment on success; otherwise <c>null</c> if the HTTP request failed or the response contained no content.
        /// </returns>
        /// <remarks>
        /// Sends a JSON POST to the "api/environment" endpoint using the configured <see cref="HttpClient"/>.
        /// The method logs an informational message at start, logs a warning if the HTTP response is unsuccessful,
        /// and returns null in failure cases. On success, the response content is deserialized to <see cref="PostEnvironmentResponse"/>.
        /// </remarks>
        public async Task<PostEnvironmentResponse?> CreateEnvironmentAsync(PostEnvironmentRequest request)
        {
            _logger?.LogInformation("EnvironmentApiClient::CreateEnvironmentAsync() start! Name: {Name}", request.Name);

            var response = await _httpClient.PostAsJsonAsync("api/environment", request);
            if (!response.IsSuccessStatusCode)
            {
                _logger?.LogWarning("EnvironmentApiClient::CreateEnvironmentAsync() failed with status code {StatusCode}.", response.StatusCode);

                return null;
            }

            var result = response.Content.ReadFromJsonAsync<PostEnvironmentResponse>().Result;
            if (null == result)
            {
                _logger?.LogWarning("EnvironmentApiClient::CreateEnvironmentAsync() returned null.");

                return null;
            }
            return result;
        }
    }
}
