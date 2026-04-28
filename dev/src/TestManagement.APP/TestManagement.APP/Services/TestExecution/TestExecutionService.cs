using Microsoft.AspNetCore.WebUtilities;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using TestManagement.APP.ApiClients;
using TestManagement.APP.ApiClients.Environment;
using TestManagement.APP.Dto.Environment.Get;
using TestManagement.APP.Dto.TestExecution.Get;
using TestManagement.APP.Dto.TestExecution.Post;
using TestManagement.APP.ViewModel.Environment;
using TestManagement.APP.ViewModel.Executions;

namespace TestManagement.APP.Services.TestExecution
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

        public virtual async Task<PostTestExecutionResponse?> CreateExecutionAsync(DateTime executedAt, string environment, string revision)
        {
            _logger.LogInformation("TestExecutionService::CreateExecutionAsync() start!");

            var request = new Dto.TestExecution.Post.PostTestExecutionRequest
            {
                ExecutedAt = executedAt,
                Environment = environment,
                Revision = revision
            };

            var result = await _apiClient.CreateTestExecutionAsync(request);

            return result;
        }
    }
}
