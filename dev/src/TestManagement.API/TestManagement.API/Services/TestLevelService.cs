using TestManagement.API.Data.Repositories;
using TestManagement.API.Models;

namespace TestManagement.API.Services
{
    /// <summary>
    /// Service for managing TestLevel entities and providing related operations.
    /// </summary>
    public class TestLevelService : ITestLevelService
    {
        /// <summary>
        /// Repository used to access TestLevel data from the data store.
        /// </summary>
        private readonly ITestLevelRepository _testLevelRepository;

        /// <summary>
        /// Logger instance for recording diagnostic and trace information.
        /// </summary>
        private readonly ILogger<TestLevelService> _logger;

        /// <summary>
        /// Creates a new instance of <see cref="TestLevelService"/>.
        /// </summary>
        /// <param name="testLevelRepository">Repository for TestLevel data access.</param>
        /// <param name="logger">Logger for this service.</param>
        public TestLevelService(
            ITestLevelRepository testLevelRepository,
            ILogger<TestLevelService> logger
            )
        {
            _testLevelRepository = testLevelRepository;
            _logger = logger;
        }

        /// <summary>
        /// Retrieve all TestLevel records asynchronously.
        /// </summary>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>A collection of <see cref="TestLevel"/> instances.</returns>
        public async Task<ICollection<TestLevel>> GetAllAsync(CancellationToken ct)
        {
            _logger.LogDebug("TestLevelService::GetAllAsync() start!");

            return await _testLevelRepository.GetAllAsync();
        }
    }
}