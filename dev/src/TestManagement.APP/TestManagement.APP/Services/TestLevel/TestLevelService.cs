using Microsoft.AspNetCore.Components.Web;
using System.Linq;
using TestManagement.APP.ApiClients.TestLevel;
using TestManagement.APP.Dto.TestExecution.Get;
using TestManagement.APP.Dto.TestLevel.Get;
using TestManagement.APP.ViewModel.TestLevel;

namespace TestManagement.APP.Services.TestLevel
{
    /// <summary>
    /// Service that provides test level related operations for the application.
    /// Coordinates calls to the API client and maps DTOs to view models.
    /// </summary>
    public class TestLevelService : ITestLevelService
    {
        /// <summary>
        /// Logger used to record diagnostic information for this service.
        /// </summary>
        private readonly ILogger<TestLevelService> _logger;

        /// <summary>
        /// API client used to fetch test level information from the backend.
        /// </summary>
        private readonly ITestLevelApiClient _apiClient;

        /// <summary>
        /// Constructs a new instance of <see cref="TestLevelService"/>.
        /// </summary>
        /// <param name="logger">Logger instance provided by DI.</param>
        /// <param name="apiClient">API client for test level data.</param>
        public TestLevelService(
            ILogger<TestLevelService> logger, 
            ITestLevelApiClient apiClient) : base()
        {
            _logger = logger;
            _apiClient = apiClient;
        }

        /// <summary>
        /// Retrieves test levels from the API and maps them to <see cref="TestLevelViewModel"/> instances.
        /// Returns an empty collection if the API returns null or no entries.
        /// </summary>
        /// <returns>A collection of <see cref="TestLevelViewModel"/>.</returns>
        public async Task<ICollection<TestLevelViewModel>> GetTestLevelAsync()
        {
            _logger.LogInformation("TestLevelService::GetTestLevelAsync() start!");

            IList<GetTestLevelResponse> response = await _apiClient.GetTestLevelAsync();
            if (response == null)
            {
                return Array.Empty<TestLevelViewModel>();
            }

            // Map using SelectMany: project each response item to a single-element sequence and flatten
            var testLevels = response
                .SelectMany(_ => new[]
                {
                    new TestLevelViewModel
                    {
                        Id = _.Id,
                        Name = _.Name,
                        Description = _.Description
                    }
                })
                .ToList();

            return testLevels;
        }
    }
}
