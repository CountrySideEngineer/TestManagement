using System.Runtime.CompilerServices;
using TestManagement.APP.Dto.TestResult.Import;
using TestManagement.APP.Services.TestExecution.Register;

namespace TestManagement.APP.Services.TestExecution.Import
{
    public class ImportTestResultService : IImportTestResultService
    {
        private readonly ILogger<ImportTestResultService> _logger;

        private readonly ISyncTestCasesService _syncTestCaseSerice;

        private readonly IRegisterTestExecutionService _registerTestExecutionService;

        public ImportTestResultService(
            ISyncTestCasesService syncTestCaseSerice, 
            IRegisterTestExecutionService registerTestExecutionService,
            ILogger<ImportTestResultService> logger
            )
        {
            _syncTestCaseSerice = syncTestCaseSerice
                ?? throw new ArgumentNullException(nameof(syncTestCaseSerice));

            _registerTestExecutionService = registerTestExecutionService
                ?? throw new ArgumentNullException(nameof(registerTestExecutionService));

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task<ImportTestResultResponse> ImportTestResultAsync(ImportTestResultRequest request, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
