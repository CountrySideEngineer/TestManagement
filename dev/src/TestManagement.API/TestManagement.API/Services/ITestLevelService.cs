using TestManagement.API.Models;

namespace TestManagement.API.Services
{
    public interface ITestLevelService
    {
        Task<ICollection<TestLevel>> GetAllAsync(CancellationToken ct);
    }
}
