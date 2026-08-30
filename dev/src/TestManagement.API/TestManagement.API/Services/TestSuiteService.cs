using Microsoft.EntityFrameworkCore;
using TestManagement.API.Data;
using TestManagement.API.Features.TestSuite;
using TestManagement.API.Models;

namespace TestManagement.API.Services;

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
    /// <returns>A collection of <see cref="GetTestSuiteResponse"/> entities.</returns>
    public async Task<ICollection<GetTestSuiteResponse>> GetAllAsync(CancellationToken ct = default)
    {
        _logger?.LogDebug("TestSuiteService::GetAllAsync() start!");

        return await _context.TestSuites
            .AsNoTracking()
            .Select(ts => new GetTestSuiteResponse
            {
                Id = ts.Id,
                Name = ts.Name,
                Description = ts.Description,
                CreatedAt = ts.CreatedAt,
                UpdatedAt = ts.UpdatedAt,
                TestSuiteVersions = ts.TestSuiteCases == null
                    ? null
                    : ts.TestSuiteCases.Select(sv => new GetTestSuiteResponse.TestSuiteVersionResponse
                    {
                        Id = sv.Id,
                        Version = sv.VersionNumber.ToString(),
                        CreatedAt = sv.CreatedAt,
                        UpdatedAt = sv.UpdatedAt,
                        TestCases = sv.TestCaseVersions == null
                            ? null
                            : sv.TestCaseVersions.Select(tv => new TestManagement.API.Features.TestCases.Get.GetTestCaseResponse
                            {
                                Id = tv.TestCase != null ? tv.TestCase.Id : tv.TestCaseId,
                                Code = tv.TestCase != null ? tv.TestCase.Code : string.Empty,
                                Versions = new List<TestManagement.API.Features.TestCases.Get.GetTestCaseResponse.TestCaseVersionItem>
                                {
                                    new TestManagement.API.Features.TestCases.Get.GetTestCaseResponse.TestCaseVersionItem
                                    {
                                        Id = tv.Id,
                                        Name = tv.Name,
                                        Description = tv.Description,
                                        VersionNumber = tv.VersionNumber,
                                        TestLevelId = tv.TestLevelId,
                                        IsLatest = tv.IsLatest,
                                        CreatedAt = tv.CreatedAt,
                                        UpdatedAt = tv.UpdatedAt
                                    }
                                }
                            }).ToList()
                    }).ToList()
            })
            .ToListAsync(ct);
    }

    /// <summary>
    /// Retrieve a single test suite by its primary key identifier. Returns null
    /// when no matching record exists. Related suite versions (cases) are included
    /// in the returned entity.
    /// </summary>
    /// <param name="id">Identifier of the test suite to retrieve.</param>
    /// <param name="ct">Cancellation token to cancel the operation.</param>
    /// <returns>The matching <see cref="GetTestSuiteResponse"/> if found; otherwise null.</returns>
    public async Task<GetTestSuiteResponse?> GetByIdAsync(long id, CancellationToken ct = default)
    {
        _logger?.LogDebug("TestSuiteService::GetByIdAsync() start!");

        return await _context.TestSuites
            .Where(ts => ts.Id == id)
            .AsNoTracking()
            .Select(ts => new GetTestSuiteResponse
            {
                Id = ts.Id,
                Name = ts.Name,
                Description = ts.Description,
                CreatedAt = ts.CreatedAt,
                UpdatedAt = ts.UpdatedAt,
                TestSuiteVersions = ts.TestSuiteCases == null
                    ? null
                    : ts.TestSuiteCases.Select(sv => new GetTestSuiteResponse.TestSuiteVersionResponse
                    {
                        Id = sv.Id,
                        Version = sv.VersionNumber.ToString(),
                        CreatedAt = sv.CreatedAt,
                        UpdatedAt = sv.UpdatedAt,
                        TestCases = sv.TestCaseVersions == null
                            ? null
                            : sv.TestCaseVersions.Select(tv => new TestManagement.API.Features.TestCases.Get.GetTestCaseResponse
                            {
                                Id = tv.TestCase != null ? tv.TestCase.Id : tv.TestCaseId,
                                Code = tv.TestCase != null ? tv.TestCase.Code : string.Empty,
                                Versions = new List<TestManagement.API.Features.TestCases.Get.GetTestCaseResponse.TestCaseVersionItem>
                                {
                                    new TestManagement.API.Features.TestCases.Get.GetTestCaseResponse.TestCaseVersionItem
                                    {
                                        Id = tv.Id,
                                        Name = tv.Name,
                                        Description = tv.Description,
                                        VersionNumber = tv.VersionNumber,
                                        TestLevelId = tv.TestLevelId,
                                        IsLatest = tv.IsLatest,
                                        CreatedAt = tv.CreatedAt,
                                        UpdatedAt = tv.UpdatedAt
                                    }
                                }
                            }).ToList()
                    }).ToList()
            })
            .FirstOrDefaultAsync(ct);
    }
}
