using System.Runtime.CompilerServices;
using TestManagement.APP.ApiClients.TestResult;
using TestManagement.APP.Dto.TestResult;
using TestManagement.APP.Dto.TestResult.Post;
using TestManagement.APP.Dto.TestResult.Register;

namespace TestManagement.APP.Services.TestExecution.Register
{
    public class RegisterTestExecutionService : IRegisterTestExecutionService
    {
        private readonly ILogger<RegisterTestExecutionService> _logger;

        private readonly ITestResultApiClient _testResultApiClient;

        public RegisterTestExecutionService(
            ILogger<RegisterTestExecutionService> logger, 
            ITestResultApiClient testResultApiClient)
        {
            _logger = logger;
            _testResultApiClient = testResultApiClient;
        }

        public async Task RegisterTestExecutionAsync(
            IEnumerable<RegisterTestResultRequest> testResults,
            CancellationToken ct = default)
        {
            _logger.LogDebug("Registering test execution with {Count} test results", testResults.Count());

            var requests = new List<PostTestResultRequest>();
            foreach (var testResult in testResults)
            {
                var request = new PostTestResultRequest
                {
                    TestExecutionItemId = testResult.TestExecutionItemId,
                    TestCaseId = testResult.TestCaseId,
                    TestCaseVersionNumber = testResult.TestCaseVersionNumber,
                    TestLevelId = testResult.TestLevelId,
                    Message = testResult.Message,
                    ExecutedAt = testResult.ExecutedAt,
                    TestResultStatus = testResult.Status
                };
                requests.Add(request);
            }

            var result = await _testResultApiClient.CreateTestResultAsync(requests, ct);
        }
    }
}
