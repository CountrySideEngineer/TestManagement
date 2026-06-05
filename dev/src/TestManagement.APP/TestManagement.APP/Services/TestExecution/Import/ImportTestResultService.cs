using System.Runtime.CompilerServices;
using System.Linq;
using TestManagement.APP.Dto.TestResult;
using TestManagement.APP.Dto.TestResult.Import;
using TestManagement.APP.Services.TestCase.Sync;
using TestManagement.APP.Services.TestExecution.Register;
using TestManagement.APP.ViewModel;
using TestManagement.APP.Dto.TestResult.Post;

namespace TestManagement.APP.Services.TestExecution.Import
{
    public class ImportTestResultService : IImportTestResultService
    {
        private readonly ILogger<ImportTestResultService> _logger;

        private readonly ISyncTestCasesService _syncTestCaseSerice;

        private readonly IRegisterTestExecutionService _registerTestExecutionService;

        private readonly ISyncTestCasesService _syncTestCaseService;

        public ImportTestResultService(
            ISyncTestCasesService syncTestCaseSerice, 
            IRegisterTestExecutionService registerTestExecutionService,
            ISyncTestCasesService syncTestCasesService,
            ILogger<ImportTestResultService> logger
            )
        {
            _syncTestCaseSerice = syncTestCaseSerice
                ?? throw new ArgumentNullException(nameof(syncTestCaseSerice));

            _registerTestExecutionService = registerTestExecutionService
                ?? throw new ArgumentNullException(nameof(registerTestExecutionService));

            _syncTestCaseService = syncTestCasesService
                ?? throw new ArgumentNullException(nameof(syncTestCasesService));

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ImportTestResultResponse> ImportAsync(
            long execId,
            long testLvId,
            ImportTestResultRequest request, 
            CancellationToken ct = default)
        {
            _logger.LogInformation("Start to import test result from source {Source} with parser {Parser}", 
                request.Source.GetType().Name, request.Parser.GetType().Name);

            ArgumentNullException.ThrowIfNull(request);

            await using var stream = await request.Source.OpenAsync();
            ICollection<ParsedTestResult> parsedTestResults = await request.Parser.ParseAsync(stream, ct);

            // Sync test cases to make sure all test cases in test result exist in database
            IEnumerable<TestCaseViewModel> testCases = await _syncTestCaseService.SyncTestCasesAsync(parsedTestResults);

            // Convert test case view model to dto as PostTestResultRequest.
            var testResultRequests = new List<PostTestResultRequest>();
            foreach (var testCase in testCases)
            {
                var parsedTestResult = 
                    parsedTestResults.FirstOrDefault(r => 
                        string.Equals(r.Code, testCase.Code, StringComparison.OrdinalIgnoreCase) &&
                        string.Equals(r.Name, testCase.Name, StringComparison.OrdinalIgnoreCase));

                var requestItem = new PostTestResultRequest
                {
                    TestExecutionItemId = execId,
                    TestCaseId = testCase.Id,
                    TestCaseVersionNumber = testCase.VersionNumber,
                    TestLevelId = testLvId,
                    Message = string.Empty,
                    ExecutedAt = parsedTestResult!.ExecutedAt,
                    TestResultStatus = parsedTestResult.Status
                };
                testResultRequests.Add(requestItem);
            }

            // Register test execution for each test result
            await _registerTestExecutionService.RegisterTestExecutionAsync(parsedTestResults, ct);

            return null;
        }
    }
}
