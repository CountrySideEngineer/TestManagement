using System.Runtime.CompilerServices;
using TestManagement.API.Features.TestSuite.Get;
using TestManagement.API.Models;

namespace TestManagement.API.Services
{
    public class TestSuiteFacadeService : ITestSuiteFacadeService
    {
        private readonly ITestSuiteService _testSuiteService;
        private readonly ITestSuiteCompositionService _testSuiteCompositionService;
        private readonly ILogger<TestSuiteFacadeService> _logger;

        public TestSuiteFacadeService(
            ITestSuiteService testSuiteService,
            ITestSuiteCompositionService testSuiteCompositionService,
            ILogger<TestSuiteFacadeService> logger)
        {
            _testSuiteService = testSuiteService;
            _testSuiteCompositionService = testSuiteCompositionService;
            _logger = logger;
        }

        public Task<ICollection<GetTestSuiteResponse>> GetAllAsync(CancellationToken ct = default)
        {
            _logger?.LogDebug("TestSuiteFacadeService::GetAllAsync start");

            var testSuites = _testSuiteService.GetAllAsync(ct);

            return testSuites;
        }

        public Task<GetTestSuiteResponse> GetByIdAsync(long id, CancellationToken ct = default)
        {
            _logger?.LogDebug("TestSuiteFacadeService::GetByIdAsync start");

            var testSuite = _testSuiteService.GetByIdAsync(id, ct);
            return testSuite;
        }

        public Task<ICollection<TestSuiteComposition>> CreateCompositionAsync(long projectId, long testCaseId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
