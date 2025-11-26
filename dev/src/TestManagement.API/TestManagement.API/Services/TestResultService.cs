using TestManagement.API.Data.Repositories;
using TestManagement.API.Models;

namespace TestManagement.API.Services
{
    public class TestResultService
    {
        private readonly ITestResultRepository _testResultRepository;

        public TestResultService(ITestResultRepository testResultRepository)
        {
            _testResultRepository = testResultRepository;
        }

        public async Task<ICollection<Models.TestResult>> GetAllAsync()
        {
            return await _testResultRepository.GetAllAsyc();
        }

        public async Task<Models.TestResult> GetByIdAsync(int id)
        {
            return await _testResultRepository.GetByIdAsync(id);
        }

        public async Task Create(Models.TestResult result)
        {
            await _testResultRepository.AddAsync(result);
        }

        public async Task Create(ICollection<TestResult> results)
        {
            await _testResultRepository.AddAsync(results);
        }
    }
}
