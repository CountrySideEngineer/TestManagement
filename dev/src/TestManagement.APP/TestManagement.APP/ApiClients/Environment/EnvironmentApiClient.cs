using System.Runtime.CompilerServices;
using TestManagement.APP.Dto.Environment.Get;

namespace TestManagement.APP.ApiClients.Environment
{
    /// <summary>
    /// API client responsible for retrieving environment information from the Test API.
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
    }
}
