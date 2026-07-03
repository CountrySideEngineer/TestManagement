using TestManagement.API.Features.TestLevel.Get;
using TestManagement.API.Models;

namespace TestManagement.API.Services
{
    public interface ITestLevelService
    {
        Task<ICollection<GetTestLevelResponse>> GetAllAsync(CancellationToken ct);
    }
}
