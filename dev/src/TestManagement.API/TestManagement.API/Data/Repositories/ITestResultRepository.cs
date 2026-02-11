using TestManagement.API.Models;
using TestManagement.API.Models.Report.Xml;

namespace TestManagement.API.Data.Repositories
{
    public interface ITestResultRepository
    {
        Task<ICollection<TestResult>> GetAllAsyc();

        Task<TestResult> GetByIdAsync(int id);

        Task AddAsync(TestResult result);

        Task AddAsync(ICollection<TestResult> results);

        Task AddAsync(TestSuitesXml suites);
    }
}
