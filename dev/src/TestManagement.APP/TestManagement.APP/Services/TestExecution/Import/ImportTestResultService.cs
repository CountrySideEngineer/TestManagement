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

            // Convert test case 







            // Map VersionNumber from synchronized TestCaseViewModel back to parsed test results
            var versionByCode = testCases
                .Where(tc => !string.IsNullOrEmpty(tc.Code))
                .ToDictionary(tc => tc.Code, tc => tc.VersionNumber, StringComparer.OrdinalIgnoreCase);

            var requests = new List<PostTestResultRequest>();
            foreach (var parsed in parsedTestResults)
            {
                if (!string.IsNullOrEmpty(parsed.Code) && versionByCode.TryGetValue(parsed.Code, out var version))
                {
                    parsed.VersionNumber = version;
                }
                //var requestItem = new PostTestResultRequest
                //{
                //    ActualResult = string.Empty,
                //    TestCaseVersionId = versionByCode.TryGetValue(parsed.Code, out var ver) ? ver : 0,
                //    ExecutedAt = parsed.ExecutedAt,
                //    Message = parsed.Name,
                //    StatusId = MapStatus(parsed.Status),
                //    TestExecutionItemId = 0 // This will be set in RegisterTestExecutionService
                //}
                // Set environment and test identifiers from method arguments
                parsed.ExecId = execId;
                parsed.TestLvId = testLvId;
            }

            // Register test execution for each test result
            await _registerTestExecutionService.RegisterTestExecutionAsync(parsedTestResults, ct);

            return null;
        }
    }
}
