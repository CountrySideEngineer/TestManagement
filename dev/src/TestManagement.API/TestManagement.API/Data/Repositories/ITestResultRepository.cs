using TestManagement.API.Models;

namespace TestManagement.API.Data.Repositories
{
    public interface ITestResultRepository
    {
        Task<ICollection<TestResult>> GetAllAsyc();

        Task<TestResult> GetByIdAsync(int id);

        Task AddAsync(TestResult result);

        Task AddAsync(ICollection<TestResult> results);
    }
}
