using System;
using TestManagement.APP.ApiClients.Environment;
using TestManagement.APP.Dto.Environment.Get;
using TestManagement.APP.ViewModel.Environment;

namespace TestManagement.APP.Services.Environment
{
    public class EnvironmentService : IEnvironmentService
    {
        private readonly ILogger<EnvironmentService> _logger;

        private readonly IEnvironmentApiClient _apiClient;

        public EnvironmentService(
            ILogger<EnvironmentService> logger,
            IEnvironmentApiClient apiClient)
        {
            _logger = logger;
            _apiClient = apiClient;
        }

        public async Task<ICollection<EnvironmentModel>?> GetEnvironmentsAsync()
        {
            _logger.LogDebug("EnvironmentService::GetEnvironmentsAsync() start!");

            IList<GetEnvironmentResponse> environmentResponses = await _apiClient.GetEnvironmentsAsync();
            IList<EnvironmentModel> environmentViewModels = environmentResponses.Select(environmentResponse => new EnvironmentModel
            {
                EnvironmentId = environmentResponse.EnvironmentId,
                DisplayName = $"{environmentResponse.Name} / {environmentResponse.Os} - ({environmentResponse.RunTime})"
            }).ToList();

            return environmentViewModels;
        }
    }
}
