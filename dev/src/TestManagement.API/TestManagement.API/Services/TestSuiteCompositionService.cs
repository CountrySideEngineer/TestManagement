using Microsoft.EntityFrameworkCore;
using TestManagement.API.Data;
using TestManagement.API.Models;

namespace TestManagement.API.Services;

/// <summary>
/// Service that provides read operations for test suite composition entities.
/// Responsible for fetching TestSuiteComposition records including related
/// TestCaseVersion and TestSuite navigation properties.
/// </summary>
public class TestSuiteCompositionService : ITestSuiteCompositionService
{
    /// <summary>
    /// Entity Framework Core database context for test management data access.
    /// </summary>
    private readonly TestManagementDbContext _context;

    /// <summary>
    /// Optional logger for diagnostic messages produced by this service.
    /// </summary>
    private readonly ILogger<TestSuiteCompositionService>? _logger;

    /// <summary>
    /// Creates a new instance of <see cref="TestSuiteCompositionService"/>.
    /// </summary>
    /// <param name="context">The database context used to query TestSuiteComposition entities.</param>
    /// <param name="logger">Logger instance for recording diagnostic information.</param>
    public TestSuiteCompositionService(TestManagementDbContext context, ILogger<TestSuiteCompositionService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves all test suite version records from the database.
    /// The returned entities include related test case versions and the owning test suite.
    /// This method performs a no-tracking query and supports cancellation via the provided token.
    /// </summary>
    /// <param name="ct">Cancellation token used to cancel the asynchronous operation.</param>
    /// <returns>A collection of <see cref="TestSuiteComposition"/> entities.</returns>
    public virtual async Task<ICollection<TestSuiteComposition>> GetAllAsync(CancellationToken ct = default)
    {
        _logger?.LogDebug("TestSuiteVersionService::GetAllAsync() start!");

        return await _context.TestSuiteCompositions
            .Include(tsv => tsv.TestCaseVersions)
            .Include(tsv => tsv.TestSuite)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    /// <summary>
    /// Retrieves a single test suite version by its primary key identifier.
    /// Includes related test case versions and the owning test suite in the result.
    /// Returns null when no matching entity is found.
    /// </summary>
    /// <param name="id">Primary key identifier of the test suite version.</param>
    /// <param name="ct">Cancellation token used to cancel the asynchronous operation.</param>
    /// <returns>The matching <see cref="TestSuiteComposition"/> or null if not found.</returns>
    public virtual async Task<TestSuiteComposition?> GetById(long id, CancellationToken ct = default)
    {
        _logger?.LogDebug("TestSuiteVersionService::GetById() start!");

        return await _context.TestSuiteCompositions
            .Where(v => v.Id == id)
            .Include(v => v.TestCaseVersions)
            .Include(v => v.TestSuite)
            .AsNoTracking()
            .FirstOrDefaultAsync(ct);
    }
}
