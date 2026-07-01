using TestManagement.API.Data.Repositories;
using TestManagement.API.Models;

namespace TestManagement.API.Services
{
    public class TestLevelService : ITestLevelService
    {
        private readonly ITestLevelRepository _testLevelRepository;
        private readonly ILogger<TestLevelService> _logger;

        public TestLevelService(
            ITestLevelRepository testLevelRepository,
            ILogger<TestLevelService> logger
            )
        {
            _testLevelRepository = testLevelRepository;
            _logger = logger;
        }

        public async Task<ICollection<TestLevel>> GetAllAsync(CancellationToken ct)
        {
            _logger.LogDebug("TestLevelService::GetAllAsync() start!");

            return await _testLevelRepository.GetAllAsync();
        }
    }
}
