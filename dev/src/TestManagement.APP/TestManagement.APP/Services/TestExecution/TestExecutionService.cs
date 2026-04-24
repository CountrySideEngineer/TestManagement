using Microsoft.AspNetCore.WebUtilities;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using TestManagement.APP.ApiClients;
using TestManagement.APP.ApiClients.Environment;
using TestManagement.APP.Dto.Environment.Get;
using TestManagement.APP.Dto.TestExecution.Get;
using TestManagement.APP.Services.TestExecution;
using TestManagement.APP.ViewModel.Environment;
using TestManagement.APP.ViewModel.Executions;

namespace TestManagement.APP.Services
{
    public class TestExecutionService : ITestExecutionService
    {
        private readonly ILogger<TestExecutionService> _logger;

        private readonly ITestExecutionApiClient _apiClient;

        private readonly IEnvironmentApiClient _envApiClient;

        public TestExecutionService(
            ILogger<TestExecutionService> logger,
            ITestExecutionApiClient apiClient,
            IEnvironmentApiClient envApiClient)
        {
            _logger = logger;
            _apiClient = apiClient;
            _envApiClient = envApiClient;
        }

        public virtual async Task<ICollection<ExecutionIndexViewModel>?> GetExecutionsAsync()
        {
            _logger.LogInformation("TestExecutionService::GetExecutionAsync() start!");

            ICollection<GetTestExecutionResponse>? testExecResponses = await _apiClient.GetTestExecutionsAsync();

            if (testExecResponses != null)
            {
                var viewModels = new List<ExecutionIndexViewModel>();

                foreach (var testExecResponse in testExecResponses)
                {
                    long pasedNum = testExecResponse.TestCases.Count(_ => _.IsPassed);
                    long errorNum = testExecResponse.TestCases.Count(_ => _.IsFailed);
                    long skippedNum = testExecResponse.TestCases.Count(_ => _.IsSkipped);
                    long excludedNum = testExecResponse.TestCases.Count(_ => _.IsExcluded);
                    long executedNum = pasedNum + errorNum;
                    long passedRate = executedNum > 0 ? pasedNum * 100 / executedNum : 0;
                    long failedRate = executedNum > 0 ? errorNum * 100 / executedNum : 0;

                    var viewModel = new ExecutionIndexViewModel
                    {
                        TestExecutionId = testExecResponse.TestExecutionId,
                        ExecutedAt = testExecResponse.ExecutedAt,
                        Environment = testExecResponse.Environment,
                        Revision = testExecResponse.Revision,
                        PassedNum = pasedNum,
                        ErrorNum = errorNum,
                        SkippedNum = skippedNum,
                        ExcludedNum = excludedNum,
                        ExecutedNum = executedNum,
                        SuccessRate = passedRate,
                        FailedRate = failedRate
                    };
                    viewModels.Add(viewModel);
                }

                return viewModels;
            }
            else
            {
                return null;
            }
        }

        public virtual async Task<ExecutionCreateViewModel> GetExecutionCreateViewModelsAsync()
        {
            _logger.LogInformation("TestExecutionService::GetExecutionCreateViewModelsAsync() start!");

            var viewModel = new ExecutionCreateViewModel();

            IList<GetEnvironmentResponse> environments = await _envApiClient.GetEnvironmentsAsync();
            if (null != environments)
            {
                var environmentModels = new List<EnvironmentModel>();
                foreach (var environment in environments)
                {
                    var environmentModel = new EnvironmentModel
                    {
                        EnvironmentId = environment.EnvironmentId,
                        DisplayName = $"{environment.Name} / {environment.Os} - ({environment.RunTime})"
                    };
                    environmentModels.Add(environmentModel);
                }
                viewModel.Environments = environmentModels;
            }

            return viewModel;
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
