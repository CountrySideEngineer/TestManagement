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

        public async Task CreateAsync(TestRun testRun)
        {
            _logger.LogDebug("TestRunService::Create() start!");

            await _testRunRepository.AddAsync(testRun);
        }

        public async Task CreateAsync(ICollection<TestRun> testRuns)
        {
            _logger.LogDebug("TestRunService::Create() start!");

            await _testRunRepository.AddAsync(testRuns);
        }
    }
}
