using System.Runtime.CompilerServices;
using TestManagement.APP.ApiClients;
using TestManagement.APP.Dto.TestExecution.Get;
using TestManagement.APP.ViewModel.Executions;

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

        public virtual async Task<IList<ExecutionIndexViewModel>> GetExecutionAsync()
        {
            _logger.LogInformation("TestExecutionService::GetExecutionAsync() start!");

            ICollection<GetTestExecutionResponse>? testExecResponses = await _apiClient.GetTestExecutionsAsync();

            var viewModels = new List<ExecutionIndexViewModel>();

            if (testExecResponses != null)
            {
                foreach (var testExecResponse in testExecResponses)
                {
                    var viewModel = new ExecutionIndexViewModel
                    {
                        TestExecutionId = testExecResponse.TestExecutionId,
                        ExecutedAt = testExecResponse.ExecutedAt,
                        Environment = testExecResponse.Environment,
                        Revision = testExecResponse.Revision,
                        ErrorNum = testExecResponse.TestCases.Count(_ => _.IsFailed),
                        SkippedNum = testExecResponse.TestCases.Count(_ => _.IsSkipped),
                        ExcludedNum = testExecResponse.TestCases.Count(_ => _.IsExcluded),
                        ExecutedNum = testExecResponse.TestCases.Count(_ => _.IsPassed || _.IsFailed)
                    };
                    viewModels.Add(viewModel);
                }
            }
            return viewModels;
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
