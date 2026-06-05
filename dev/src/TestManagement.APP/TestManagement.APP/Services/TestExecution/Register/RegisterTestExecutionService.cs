using System.Runtime.CompilerServices;
using TestManagement.APP.ApiClients.TestResult;
using TestManagement.APP.Dto.TestResult;
using TestManagement.APP.Dto.TestResult.Post;

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

        public async Task RegisterTestExecutionAsync(IEnumerable<ParsedTestResult> testResults, CancellationToken ct = default)
        {
            _logger.LogDebug("Registering test execution with {Count} test results", testResults.Count());

            var requests = new List<PostTestResultRequest>();
            foreach (var testResult in testResults)
            {
                var request = new PostTestResultRequest
                {
                    TestExecutionItemId = 1,
                    TestCaseVersionNumber = 1,
                    TestResultStatus = TestResultStatus.Unknown
                };
                requests.Add(request);
            }

            var result = await _testResultApiClient.CreateTestResultAsync(requests, ct);
        }
    }
}
