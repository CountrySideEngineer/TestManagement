using System.Runtime.CompilerServices;
using TestManagement.APP.ApiClients;
using TestManagement.APP.Dto.TestExecution.Get;

namespace TestManagement.APP.Services
{
    public class TestExecutionService : ITestExecutionService
    {
        private readonly ILogger<TestExecutionService> _logger;

        private readonly ITestExecutionApiClient _apiClient;

        public TestExecutionService(
            ILogger<TestExecutionService> logger,
        ITestExecutionApiClient apiClient)
        {
            _logger = logger;
            _apiClient = apiClient;
        }

        public virtual async Task<ICollection<GetTestExecutionResponse>?> GetTestExecutionsAsync()
        {
            _logger.LogInformation("TestExecutionService::GetTestExecutionsAsync() start!");

            var testExecutions = await _apiClient.GetTestExecutionsAsync();
            return testExecutions;
        }

        public virtual async Task<ICollection<GetTestExecutionResponse>?> GetTestExecutionsSummaryAsync()
        {
            _logger.LogInformation("TestExecutionService::GetTestExecutionsByTestRunIdAsync() start!");

            var testExecutions = await _apiClient.GetTestExecutionsAsync();

            return null;
        }
    }
}
