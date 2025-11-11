using TestManagement.Model;

namespace TestManagement.Data.Repositories
{
    public interface ITestSuiteRepository
    {
        Task<IEnumerable<TestSuite>> GetAllAsync();
        Task<TestSuite?> GetByIdAsync(int id);
        Task AddAsync(TestSuite suite);
        Task UpdateAsync(TestSuite suite);
        Task DeleteAsync(int id);
    }
}
