using TestManagement.API.Features.TestLevel.Get;
using TestManagement.API.Models;

namespace TestManagement.API.Services
{
    /// <summary>
    /// Service for retrieving test level information.
    /// Implementations should provide methods to read test level data from the underlying store.
    /// </summary>
    public interface ITestLevelService
    {
        /// <summary>
        /// Retrieve all test levels.
        /// </summary>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>A collection of <see cref="GetTestLevelResponse"/> representing available test levels.</returns>
        Task<ICollection<GetTestLevelResponse>> GetAllAsync(CancellationToken ct);
    }
}
