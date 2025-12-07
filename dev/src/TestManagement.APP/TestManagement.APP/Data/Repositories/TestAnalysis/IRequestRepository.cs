using TestManagement.APP.Models.TestAnalysis;

namespace TestManagement.APP.Data.Repositories.TestAnalysis
{
    public interface IRequestRepository
    {
        Task<ICollection<Request>> GetAllAsync();

        Task<Request?> GetByIdAsync(int id);

        Task AddAsync(Request testRun);

        Task AddAsync(ICollection<Request> testRuns);

        Task UpdateAsync(Request testRun);

        Task DeleteAsync(int id);
    }
}
