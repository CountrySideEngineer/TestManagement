using System.Runtime.CompilerServices;
using TestManagement.APP.ApiClients.TestResult;
using TestManagement.APP.Dto.TestResult;
using TestManagement.APP.Dto.TestResult.Post;
using TestManagement.APP.Dto.TestResult.Register;

namespace TestManagement.APP.Services.TestExecution.Register
{
    /// <summary>
    /// Service for registering test execution results.
    /// Converts test result data and communicates with the test result API client for persistence.
    /// </summary>
    public class RegisterTestExecutionService : IRegisterTestExecutionService
    {
        /// <summary>
        /// Logger for recording diagnostic information and errors.
        /// </summary>
        private readonly ILogger<RegisterTestExecutionService> _logger;

        /// <summary>
        /// API client for communicating with the test result endpoint.
        /// </summary>
        private readonly ITestResultApiClient _testResultApiClient;

        /// <summary>
        /// Constructs an instance of <see cref="RegisterTestExecutionService"/>.
        /// </summary>
        /// <param name="logger">Logger for diagnostics.</param>
        /// <param name="testResultApiClient">API client for test result operations.</param>
        public RegisterTestExecutionService(
            ILogger<RegisterTestExecutionService> logger, 
            ITestResultApiClient testResultApiClient)
        {
            _logger = logger;
            _testResultApiClient = testResultApiClient;
        }

        /// <summary>
        /// Registers test execution results by converting them to API requests and persisting them asynchronously.
        /// </summary>
        /// <param name="testResults">Collection of test results to register.</param>
        /// <param name="ct">Cancellation token for the asynchronous operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task<IEnumerable<RegisterTestResultResponse>> RegisterTestExecutionAsync(
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

            var results = await _testResultApiClient.CreateTestResultAsync(requests, ct);
            List<RegisterTestResultResponse> responses = new List<RegisterTestResultResponse>();
            foreach (var result in results)
            {
                var response = new RegisterTestResultResponse
                {
                    TestResultId = result.TestResultId,
                    TestExecutionItemId = result.TestExecutionItemId,
                    TestCaseId = result.TestCaseId,
                    TestCaseVersionNumber = result.TestCaseVersionNumber,
                    TestLevelId = result.TestLevelId,
                    Message = result.Message,
                    Status = result.TestResultStatus,
                    ExecutedAt = result.ExecutedAt
                };
                responses.Add(response);
            }
            return responses;
        }
    }
}
