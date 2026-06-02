using TestManagement.API.Models;

namespace TestManagement.API.Data.Repositories
{
    public interface ITestCaseRepository
    {
        Task<ICollection<TestCaseVersion>> GetAllAsync();

        Task<ICollection<TestCaseVersion>> GetByIdAsync(int testLevelId);

        Task AddAsync(TestCaseVersion testCase);

        Task AddAsync(ICollection<TestCaseVersion> testCases);
    }
}
