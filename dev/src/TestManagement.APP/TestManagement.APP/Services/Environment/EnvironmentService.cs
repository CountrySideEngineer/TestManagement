using System;
using TestManagement.APP.ApiClients.Environment;
using TestManagement.APP.Dto.Environment.Get;
using TestManagement.APP.ViewModel.Environment;

namespace TestManagement.APP.Services.Environment
{
    /// <summary>
    /// Service that provides environment-related operations for the application.
    /// Coordinates calls to the environment API client and maps DTOs to view models.
    /// </summary>
    public class EnvironmentService : IEnvironmentService
    {
        /// <summary>
        /// Logger used to record diagnostic information for this service.
        /// </summary>
        private readonly ILogger<EnvironmentService> _logger;

        /// <summary>
        /// API client used to fetch environment information from the backend.
        /// </summary>
        private readonly IEnvironmentApiClient _apiClient;

        /// <summary>
        /// Constructs a new instance of <see cref="EnvironmentService"/>.
        /// </summary>
        /// <param name="logger">Logger instance provided by DI.</param>
        /// <param name="apiClient">API client for environment data.</param>
        public EnvironmentService(
            ILogger<EnvironmentService> logger,
            IEnvironmentApiClient apiClient)
        {
            _logger = logger;
            _apiClient = apiClient;
        }

        /// <summary>
        /// Retrieves environments from the API and maps them to <see cref="EnvironmentViewModel"/> instances.
        /// Returns null if the API client returns null.
        /// </summary>
        /// <returns>A collection of <see cref="EnvironmentViewModel"/>, or null.</returns>
        public async Task<ICollection<EnvironmentViewModel>?> GetEnvironmentsAsync()
        {
            _logger.LogDebug("EnvironmentService::GetEnvironmentsAsync() start!");

            IList<GetEnvironmentResponse> environmentResponses = await _apiClient.GetEnvironmentsAsync();
            IList<EnvironmentViewModel> environmentViewModels = environmentResponses.Select(environmentResponse => new EnvironmentViewModel
            {
                EnvironmentId = environmentResponse.EnvironmentId,
                Name = environmentResponse.Name,
                Os = environmentResponse.Os,
                RunTime = environmentResponse.RunTime,
                DisplayName = $"{environmentResponse.Name} / {environmentResponse.Os} - ({environmentResponse.RunTime})"
            }).ToList();

            return environmentViewModels;
        }
    }
}
