using System.Linq;
using System.Runtime.CompilerServices;
using TestManagement.APP.Dto.TestResult;
using TestManagement.APP.Dto.TestResult.Import;
using TestManagement.APP.Dto.TestResult.Post;
using TestManagement.APP.Dto.TestResult.Register;
using TestManagement.APP.Services.TestCase.Sync;
using TestManagement.APP.Services.TestExecution.Register;
using TestManagement.APP.ViewModel;

namespace TestManagement.APP.Services.TestExecution.Import
{
    /// <summary>
    /// Service for importing test results from various sources.
    /// Handles parsing, synchronizing test cases, and registering test execution results.
    /// </summary>
    public class ImportTestResultService : IImportTestResultService
    {
        /// <summary>
        /// Logger for recording diagnostic information and errors.
        /// </summary>
        private readonly ILogger<ImportTestResultService> _logger;

        /// <summary>
        /// Service for synchronizing test cases to ensure they exist in the database.
        /// </summary>
        private readonly ISyncTestCasesService _syncTestCaseSerice;

        /// <summary>
        /// Service for registering test execution results.
        /// </summary>
        private readonly IRegisterTestExecutionService _registerTestExecutionService;

        /// <summary>
        /// Service for synchronizing test cases (duplicate of <see cref="_syncTestCaseSerice"/> for consistency).
        /// </summary>
        private readonly ISyncTestCasesService _syncTestCaseService;

        /// <summary>
        /// Constructs an instance of <see cref="ImportTestResultService"/>.
        /// </summary>
        /// <param name="syncTestCaseSerice">Service for synchronizing test cases.</param>
        /// <param name="registerTestExecutionService">Service for registering test execution results.</param>
        /// <param name="syncTestCasesService">Alternative service for synchronizing test cases.</param>
        /// <param name="logger">Logger for diagnostics.</param>
        /// <exception cref="ArgumentNullException">Thrown if any required dependency is null.</exception>
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

        /// <summary>
        /// Imports test results from the provided source asynchronously.
        /// Parses the source data, synchronizes test cases, and registers the test execution results.
        /// </summary>
        /// <param name="execId">Identifier of the test execution.</param>
        /// <param name="execItemId">Identifier of the test execution item.</param>
        /// <param name="testLvId">Identifier of the test level.</param>
        /// <param name="request">Import request containing the source and parser.</param>
        /// <param name="ct">Cancellation token for the asynchronous operation.</param>
        /// <returns>An <see cref="ImportTestResultResponse"/> indicating the result of the import.</returns>
        public async Task<ImportTestResultResponse> ImportAsync(
            long execId,
            long execItemId,
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
            var testResultRequests = new List<RegisterTestResultRequest>();
            foreach (var testCase in testCases)
            {
                var parsedTestResult = 
                    parsedTestResults.FirstOrDefault(r => 
                        string.Equals(r.Code, testCase.Code, StringComparison.OrdinalIgnoreCase) &&
                        string.Equals(r.Name, testCase.Name, StringComparison.OrdinalIgnoreCase));

                var requestItem = new RegisterTestResultRequest
                {
                    TestExecutionItemId = execItemId,
                    TestCaseId = testCase.Id,
                    TestCaseVersionNumber = testCase.VersionNumber,
                    TestLevelId = testLvId,
                    Message = string.Empty,
                    ExecutedAt = parsedTestResult!.ExecutedAt,
                    Status = parsedTestResult.Status.ToString()
                };
                testResultRequests.Add(requestItem);
            }

            // Register test execution for each test result
            await _registerTestExecutionService.RegisterTestExecutionAsync(testResultRequests, ct);

            return null;
        }
    }
}
