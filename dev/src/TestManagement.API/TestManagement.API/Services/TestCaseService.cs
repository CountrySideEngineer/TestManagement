using TestManagement.API.Data.Repositories;
using TestManagement.API.Models;

namespace TestManagement.API.Services
{
    public class TestCaseService
    {
        private readonly ITestCaseRepository _testCaseRepository;
        private readonly ILogger<TestCaseService> _logger;

        public TestCaseService(
            ITestCaseRepository testCaseRepository, 
            ILogger<TestCaseService> logger)
        {
            _testCaseRepository = testCaseRepository;
            _logger = logger;
        }

        public async Task<ICollection<TestCaseVersion>> GetAllAsync()
        {
            _logger.LogDebug("TestCaseService::GetAllAsync() start!");

            return await _testCaseRepository.GetAllAsync();
        }

        public async Task<ICollection<TestCaseVersion>> GetByIdAsync(int testLevelId)
        {
            _logger.LogDebug("TestCaseService::GetByIdAsync() start!");

            return await _testCaseRepository.GetByIdAsync(testLevelId);
        }

        public async Task CreateAsync(TestCaseVersion testCase)
        {
            _logger.LogDebug("TestCaseService::CreateAsync() start!");

            await _testCaseRepository.AddAsync(testCase);
        }

        public async Task CreateAsync(ICollection<TestCaseVersion> testCases)
        {
            _logger.LogDebug("TestCaseService::Create() start!");

            await _testCaseRepository.AddAsync(testCases);
        }
    }
}
