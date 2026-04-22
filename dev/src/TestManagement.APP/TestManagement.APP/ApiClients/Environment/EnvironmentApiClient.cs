using System.Runtime.CompilerServices;
using TestManagement.APP.Dto.Environment.Get;

namespace TestManagement.APP.ApiClients.Environment
{
    public class EnvironmentApiClient : IEnvironmentApiClient
    {
        private readonly ILogger<EnvironmentApiClient> _logger;

        private readonly HttpClient _httpClient;

        public EnvironmentApiClient(
            ILogger<EnvironmentApiClient> logger,
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient("TestApiClient");
        }

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
