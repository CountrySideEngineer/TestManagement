using TestManagement.API.Models;

namespace TestManagement.API.Data.Repositories
{
    public interface ITestRunRepository
    {
        Task<ICollection<TestRun>> GetAllAsync();

        Task<TestRun?> GetByIdAsync(int id);

        Task AddAsync(TestRun testRun);

        Task AddAsync(ICollection<TestRun> testRuns);

        Task UpdateAsync(TestRun testRun);

        Task DeleteAsync(int id);
    }
}
