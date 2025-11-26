using TestManagement.API.Models;

namespace TestManagement.API.Data.Repositories
{
    public interface ITestLevelRepository
    {
        Task<ICollection<TestLevel>> GetAllAsync();
    }
}
