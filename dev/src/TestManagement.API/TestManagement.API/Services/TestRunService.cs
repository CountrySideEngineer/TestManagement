using TestManagement.API.Data.Repositories;
using TestManagement.API.Models;

namespace TestManagement.API.Services
{
    public class TestRunService
    {
        private readonly ITestRunRepository _testRunRepository;
        private readonly ILogger<TestRunService> _logger;

        public TestRunService(
            ILogger<TestRunService> logger,
            ITestRunRepository testRunRepository)
        {
            _logger = logger;
            _testRunRepository = testRunRepository;
        }

        public async Task<ICollection<TestRun>> GetAllAsync()
        {
            _logger.LogDebug("TestRunService::GetAllAsync() start!");

            return await _testRunRepository.GetAllAsync();
        }

        public async Task<TestRun?> GetByIdAsync(int id)
        {
            _logger.LogDebug("TestRunService::GetByIdAsync() start!");

            return await _testRunRepository.GetByIdAsync(id);
        }

        public async Task<TestRun?> GetByAbstractAndEnvAsync(TestRun testRun)
        {
            _logger.LogDebug("TestRunService::GetByAbstractAndEnvAsync() start!");

            var existingRuns = await _testRunRepository.GetAllAsync();
            var match = existingRuns.FirstOrDefault(tr =>
                string.Equals(tr.Abstract, testRun.Abstract, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(tr.Environment, testRun.Environment, StringComparison.OrdinalIgnoreCase));

            return match;
        }

        public async Task<TestRun> CreateAsync(TestRun testRun)
        {
            _logger.LogDebug("TestRunService::Create() start!");

            // Check for duplicate TestRun by Abstract and Environment
            var duplicatedTestRun = await GetByAbstractAndEnvAsync(testRun);
            if (null != duplicatedTestRun)
            {
                _logger.LogWarning("Duplicate TestRun detected. Abstract: {Abstract}, Environment: {Environment}", testRun.Abstract, testRun.Environment);

                return duplicatedTestRun;
            }
            else
            {
                var created = await _testRunRepository.AddAsync(testRun);
                return created;

            }
        }
    }
}
