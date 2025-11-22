using TestManagement.API.Data.Repositories;
using TestManagement.API.Models;

namespace TestManagement.API.Services
{
    public class TestCaseService
    {
        private readonly ITestCaseRepository _testCaseRepository;

        public TestCaseService(ITestCaseRepository testCaseRepository)
        {
            _testCaseRepository = testCaseRepository;
        }

        public async Task<ICollection<TestCase>> GetAllAsync()
        {
            return await _testCaseRepository.GetAllAsync();
        }

        public async Task<ICollection<TestCase>> GetByIdAsync(int testLevelId)
        {
            return await _testCaseRepository.GetByIdAsync(testLevelId);
        }
    }
}
