using TestManagement.APP.ApiClients;
using TestManagement.APP.Dto.TestExecution.Get;
using TestManagement.APP.Services.TestExecution;
using TestManagement.APP.ViewModel.Dashboard;

namespace TestManagement.APP.Services
{
    /// <summary>
    /// Service used by the application to build dashboard view models.
    /// Aggregates test execution data and projects it into view models consumed by the UI.
    /// </summary>
    public class DashboardService : IDashboardService
    {
        /// <summary>
        /// Logger instance used to produce diagnostic messages for the service.
        /// </summary>
        private readonly ILogger<TestExecutionService> _logger;

        /// <summary>
        /// API client used to retrieve test execution DTOs from the backend.
        /// </summary>
        private readonly ITestExecutionApiClient _apiClient;

        /// <summary>
        /// Constructs a new <see cref="DashboardService"/> instance.
        /// </summary>
        /// <param name="logger">Logger provided by dependency injection.</param>
        /// <param name="apiClient">API client used to query test executions.</param>
        public DashboardService(
            ILogger<TestExecutionService> logger, 
            ITestExecutionApiClient apiClient
            )
        {
            _logger = logger;
            _apiClient = apiClient;
        }

        /// <summary>
        /// Builds the dashboard view model by fetching test executions and calculating
        /// aggregated metrics for the latest run and several recent runs.
        /// </summary>
        /// <returns>A <see cref="DashboardViewModel"/> containing the latest execution summary and recent executions.</returns>
        public virtual async Task<DashboardViewModel> GetAsync()
        {
            _logger.LogDebug("DashboardService::GetAsync() start!");

            var testExecution = await _apiClient.GetTestExecutionsAsync();

            GetTestExecutionResponse latestExecution = testExecution?.OrderByDescending(_ => _.ExecutedAt).FirstOrDefault()!;

            long errNum = latestExecution.TestCases.Where(_ => _.IsFailed).Count();
            long skipped = latestExecution.TestCases.Where(_ => _.IsSkipped).Count();
            long excluded = latestExecution.TestCases.Where(_ => _.IsExcluded).Count();
            long passed = latestExecution.TestCases.Where(_ => _.IsPassed).Count();

            var latestSummaryViewModel = new LatestExecutionSummaryViewModel
            {
                Revision = latestExecution.Revision,
                EnvironmentName = latestExecution.Environment,
                ErrorNum = errNum,
                SkippedNum = skipped,
                ExcludedNum = excluded,
                ExecutedNum = (errNum + passed),
                SuccessRate = (errNum + passed) > 0 ? (passed * 100 / (errNum + passed)) : 0,
                FailedRate = (errNum + passed) > 0 ? (errNum * 100 / (errNum + passed)) : 0,
                ExecutedAt = latestExecution?.ExecutedAt ?? DateTime.MinValue
            };
            var recentExecutions = testExecution?.OrderByDescending(_ => _.ExecutedAt).Take(new Range(1, 5)).Select(_ => new RecentExecutionViewModel()
            {
                Revision = _.Revision,
                ExecutedAt = _.ExecutedAt,
                HasFailure = _.TestCases.Any(_ => _.TestStatusCode == "Failed")
            });

            var viewModel = new DashboardViewModel
            {
                LatestExecutionSummary = latestSummaryViewModel,
                RecentExecutions = recentExecutions
            };
            return viewModel;
        }
    }
}
