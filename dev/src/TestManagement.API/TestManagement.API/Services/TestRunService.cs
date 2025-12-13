using TestManagement.API.Data.Repositories;
using TestManagement.API.Models;

namespace TestManagement.API.Services
{
    public class TestRunService
    {
        private readonly ITestRunRepository _testRunRepository;

        public TestRunService(ITestRunRepository testRunRepository)
        {
            _testRunRepository = testRunRepository;
        }

        public async Task<ICollection<TestRun>> GetAllAsync()
        {
            return await _testRunRepository.GetAllAsync();
        }

        public async Task<TestRun?> GetByIdAsync(int id)
        {
            return await _testRunRepository.GetByIdAsync(id);
        }

        public async Task Create(TestRun testRun)
        {
            await _testRunRepository.AddAsync(testRun);
        }

        public async Task Create(ICollection<TestRun> testRuns)
        {
            await _testRunRepository.AddAsync(testRuns);
        }
    }
}
