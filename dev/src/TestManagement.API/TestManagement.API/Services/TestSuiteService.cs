using Microsoft.EntityFrameworkCore;
using TestManagement.API.Data;
using TestManagement.API.Models;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace TestManagement.API.Services
{
    /// <summary>
    /// Application service that provides operations to read <see cref="TestSuite"/> entities
    /// from the persistence store. This service is a thin wrapper over the
    /// <see cref="TestManagementDbContext"/> to perform read-only queries related to test suites.
    /// </summary>
    public class TestSuiteService : ITestSuiteService
    {
        /// <summary>
        /// EF Core database context used to access TestSuite related DbSets and execute queries.
        /// </summary>
        private readonly TestManagementDbContext _context;

        /// <summary>
        /// Optional logger used to record diagnostic information for this service.
        /// </summary>
        private readonly ILogger<TestSuiteService>? _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestSuiteService"/> class.
        /// </summary>
        /// <param name="context">The database context to use for queries.</param>
        /// <param name="logger">Optional logger instance.</param>
        public TestSuiteService(TestManagementDbContext context, ILogger<TestSuiteService>? logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Retrieve all test suites from the database. Related suite versions (cases)
        /// are included in the query result. The query is executed as a no-tracking
        /// operation to optimize read-only scenarios.
        /// </summary>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>A collection of <see cref="TestSuite"/> entities.</returns>
        public async Task<ICollection<TestSuite>> GetAllAsync(CancellationToken ct = default)
        {
            _logger?.LogDebug("TestSuiteService::GetAllAsync() start!");

            return await _context.TestSuites
                .Include(ts => ts.TestSuiteCases)
                .AsNoTracking()
                .ToListAsync(ct);
        }

        /// <summary>
        /// Retrieve a single test suite by its primary key identifier. Returns null
        /// when no matching record exists. Related suite versions (cases) are included
        /// in the returned entity.
        /// </summary>
        /// <param name="id">Identifier of the test suite to retrieve.</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>The matching <see cref="TestSuite"/> if found; otherwise null.</returns>
        public async Task<TestSuite?> GetByIdAsync(long id, CancellationToken ct = default)
        {
            _logger?.LogDebug("TestSuiteService::GetByIdAsync() start!");

            return await _context.TestSuites
                .Where(ts => ts.Id == id)
                .Include(ts => ts.TestSuiteCases)
                .AsNoTracking()
                .FirstOrDefaultAsync(ct);
        }
    }
}
