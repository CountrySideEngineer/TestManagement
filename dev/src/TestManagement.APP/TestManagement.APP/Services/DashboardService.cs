using TestManagement.APP.ApiClients;
using TestManagement.APP.Dto.TestExecution.Get;
using TestManagement.APP.ViewModel.Dashboard;

namespace TestManagement.APP.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ILogger<TestExecutionService> _logger;

        private readonly ITestExecutionApiClient _apiClient;

        public DashboardService(
            ILogger<TestExecutionService> logger, 
            ITestExecutionApiClient apiClient
            )
        {
            _logger = logger;
            _apiClient = apiClient;
        }

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
