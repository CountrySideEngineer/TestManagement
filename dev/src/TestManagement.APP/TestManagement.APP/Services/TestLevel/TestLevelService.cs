using Microsoft.AspNetCore.Components.Web;
using System.Linq;
using TestManagement.APP.ApiClients.TestLevel;
using TestManagement.APP.Dto.TestExecution.Get;
using TestManagement.APP.Dto.TestLevel.Get;
using TestManagement.APP.ViewModel.TestLevel;

namespace TestManagement.APP.Services.TestLevel
{
    public class TestLevelService : ITestLevelService
    {
        private readonly ILogger<TestLevelService> _logger;

        private readonly ITestLevelApiClient _apiClient;

        public TestLevelService(
            ILogger<TestLevelService> logger, 
            ITestLevelApiClient apiClient) : base()
        {
            _logger = logger;
            _apiClient = apiClient;
        }

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
