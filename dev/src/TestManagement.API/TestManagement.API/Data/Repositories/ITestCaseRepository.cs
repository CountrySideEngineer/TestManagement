using TestManagement.API.Models;

namespace TestManagement.API.Data.Repositories
{
    public interface ITestCaseRepository
    {
        Task<ICollection<TestCase>> GetAllAsync();

        Task<ICollection<TestCase>> GetByIdAsync(int testLevelId);

        Task AddAsync(TestCase testCase);

        Task AddAsync(ICollection<TestCase> testCases);
    }
}
