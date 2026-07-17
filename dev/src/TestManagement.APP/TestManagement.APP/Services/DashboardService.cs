using System.Text.Json;
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
        public virtual async Task<DashboardViewModel?> GetAsync()
        {
            _logger.LogDebug("DashboardService::GetAsync() start!");

            try
            {
                var testExecution = await _apiClient.GetTestExecutionsAsync();
                if ((null == testExecution) || (!testExecution.Any()))
                {
                    _logger.LogWarning("No test execution found.");

                    return null;
                }

                GetTestExecutionResponse? latestExecution = testExecution?.OrderByDescending(_ => _.ExecutedAt).FirstOrDefault();
                if (null == latestExecution)
                {
                    _logger.LogWarning("No test executions found.");
                    return null;
                }
                else
                {
                    long errNum = latestExecution.TestCases.Where(_ => _.IsFailed).Count();
                    long skipped = latestExecution.TestCases.Where(_ => _.IsSkipped).Count();
                    long excluded = latestExecution.TestCases.Where(_ => _.IsExcluded).Count();
                    long passed = latestExecution.TestCases.Where(_ => _.IsPassed).Count();

                    var latestSummaryViewModel = new LatestExecutionSummaryViewModel
                    {
                        ExecutionId = latestExecution.TestExecutionId,
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
                        ExecutionId = _.TestExecutionId,
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
            catch (HttpRequestException ex)
            {
                _logger?.LogWarning(ex, "Connection to API failed.");
                return null;
            }
            catch (JsonException ex)
            {
                _logger?.LogWarning(ex, "Failed to parse the API response JSON.");
                return null;
            }
        }
    }
}
