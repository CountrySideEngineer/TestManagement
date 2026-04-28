using TestManagement.APP.ApiClients;
using TestManagement.APP.Dto.TestExecution.Get;
using TestManagement.APP.Dto.TestExecution.Post;
using TestManagement.APP.ViewModel.Executions;

namespace TestManagement.APP.Services.TestExecution
{
    /// <summary>
    /// Service responsible for operations related to test executions.
    /// It coordinates with an API client to fetch or create test execution data
    /// and maps DTOs to view models used by the UI.
    /// </summary>
    public class TestExecutionService : ITestExecutionService
    {
        /// <summary>
        /// Logger used for diagnostic and informational messages.
        /// </summary>
        private readonly ILogger<TestExecutionService> _logger;

        /// <summary>
        /// API client used to interact with the test execution endpoints.
        /// </summary>
        private readonly ITestExecutionApiClient _apiClient;

        /// <summary>
        /// Initializes a new instance of <see cref="TestExecutionService"/>.
        /// </summary>
        /// <param name="logger">Logger instance provided by DI.</param>
        /// <param name="apiClient">Client used to call test execution APIs.</param>
        public TestExecutionService(
            ILogger<TestExecutionService> logger,
            ITestExecutionApiClient apiClient)
        {
            _logger = logger;
            _apiClient = apiClient;
        }

        /// <summary>
        /// Retrieves executions and projects them into <see cref="ExecutionIndexViewModel"/> objects.
        /// Calculates summary statistics such as passed/failed counts and rates.
        /// </summary>
        /// <returns>A collection of execution view models, or null when no data is available.</returns>
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

        /// <summary>
        /// Returns the raw test execution DTOs from the API.
        /// </summary>
        /// <returns>A collection of <see cref="GetTestExecutionResponse"/>, or null.</returns>
        public virtual async Task<ICollection<GetTestExecutionResponse>?> GetTestExecutionsAsync()
        {
            _logger.LogInformation("TestExecutionService::GetTestExecutionsAsync() start!");

            var testExecutions = await _apiClient.GetTestExecutionsAsync();
            return testExecutions;
        }

        /// <summary>
        /// (Placeholder) Intended to retrieve a summarized view of test executions.
        /// Currently returns null.
        /// </summary>
        /// <returns>Summary collection of test executions or null.</returns>
        public virtual async Task<ICollection<GetTestExecutionResponse>?> GetTestExecutionsSummaryAsync()
        {
            _logger.LogInformation("TestExecutionService::GetTestExecutionsByTestRunIdAsync() start!");

            var testExecutions = await _apiClient.GetTestExecutionsAsync();

            return null;
        }

        /// <summary>
        /// Creates a new test execution by forwarding a request to the API client.
        /// </summary>
        /// <param name="executedAt">Execution timestamp.</param>
        /// <param name="environment">Environment name where the tests ran.</param>
        /// <param name="revision">Revision identifier (e.g. commit SHA).</param>
        /// <returns>The created execution response from the API, or null on failure.</returns>
        public virtual async Task<PostTestExecutionResponse?> CreateExecutionAsync(DateTime executedAt, string environment, string revision)
        {
            _logger.LogInformation("TestExecutionService::CreateExecutionAsync() start!");

            var request = new PostTestExecutionRequest
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
