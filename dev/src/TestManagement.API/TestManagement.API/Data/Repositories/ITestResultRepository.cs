using TestManagement.API.Models;
using TestManagement.API.Models.Report.Xml;

namespace TestManagement.API.Data.Repositories
{
    public interface ITestResultRepository
    {
        Task<ICollection<CreateTestResultRequest>> GetAllAsyc();

        Task<CreateTestResultRequest> GetByIdAsync(int id);

        Task AddAsync(CreateTestResultRequest result);

        Task AddAsync(ICollection<CreateTestResultRequest> results);

        Task AddAsync(TestSuitesXml suites);
    }
}
