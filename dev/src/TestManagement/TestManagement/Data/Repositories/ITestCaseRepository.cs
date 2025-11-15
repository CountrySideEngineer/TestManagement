using TestManagement.Model;

namespace TestManagement.Data.Repositories
{
    public interface ITestCaseRepository
    {
        Task<IEnumerable<TestCase>> GetAllAsync();
        Task<TestCase?> GetByIdAsync(int id);
        Task AddAsync(TestCase testCase);
        Task UpdateAsync(TestCase testCase);
        Task DeleteAsync(int id);
    }
}
