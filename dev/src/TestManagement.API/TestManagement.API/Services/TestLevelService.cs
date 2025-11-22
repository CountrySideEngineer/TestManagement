using TestManagement.API.Data.Repositories;
using TestManagement.API.Models;

namespace TestManagement.API.Services
{
    public class TestLevelService
    {
        private readonly ITestLevelRepository _testLevelRepository;

        public TestLevelService(ITestLevelRepository testLevelRepository)
        {
            _testLevelRepository = testLevelRepository;
        }

        public async Task<ICollection<TestLevel>> GetAllAsync()
        {
            return await _testLevelRepository.GetAllAsync();
        }
    }
}
