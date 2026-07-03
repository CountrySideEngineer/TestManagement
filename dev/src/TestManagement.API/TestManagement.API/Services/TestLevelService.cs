using Microsoft.EntityFrameworkCore;
using TestManagement.API.Data;
using TestManagement.API.Data.Repositories;
using TestManagement.API.Features.TestLevel.Get;
using TestManagement.API.Models;

namespace TestManagement.API.Services
{
    /// <summary>
    /// Service for managing TestLevel entities and providing related operations.
    /// </summary>
    public class TestLevelService : ITestLevelService
    {
        /// <summary>
        /// Database context used to access and persist test execution related entities.
        /// </summary>
        private readonly TestManagementDbContext _dbContext;

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
            TestManagementDbContext dbContext,
            ILogger<TestLevelService> logger
            )
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Retrieve all TestLevel records asynchronously.
        /// </summary>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>A collection of <see cref="TestLevel"/> instances.</returns>
        public async Task<ICollection<GetTestLevelResponse>> GetAllAsync(CancellationToken ct)
        {
            _logger.LogDebug("TestLevelService::GetAllAsync() start!");

            ICollection<TestLevel> testLevels = await _dbContext.TestLevels.ToListAsync(ct);
            ICollection<GetTestLevelResponse> response = testLevels
                .Select(_ => new GetTestLevelResponse
                {
                    Id = _.Id,
                    Name = _.Name,
                    Description = _.Description
                }).ToList();
            return response;
        }
    }
}