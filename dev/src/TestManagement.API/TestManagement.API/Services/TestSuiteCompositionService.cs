using Microsoft.EntityFrameworkCore;
using TestManagement.API.Data;
using TestManagement.API.Features.TestSuite.Create;
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
        _logger?.LogDebug("TestSuiteCompositionService::GetAllAsync() start!");

        return await _context.TestSuiteCompositions
            .Include(tsv => tsv.TestCaseVersion)
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
        _logger?.LogDebug("TestSuiteCompositionService::GetById() start!");

        return await _context.TestSuiteCompositions
            .Where(v => v.Id == id)
            .Include(v => v.TestCaseVersion)
            .Include(v => v.TestSuite)
            .AsNoTracking()
            .FirstOrDefaultAsync(ct);
    }

    /// <summary>
    /// Retrieve all TestSuiteComposition records for a given test suite id,
    /// including the related TestCaseVersion and its TestLevel.
    /// </summary>
    /// <remarks>
    /// - Uses EF Core eager loading (<c>Include</c> / <c>ThenInclude</c>) to fetch related entities in a single query.
    /// - Query is executed with <c>AsNoTracking()</c> as this is a read-only operation.
    /// - Returns <c>null</c> only if the query execution itself results in no data; callers should handle an empty collection as appropriate.
    /// - Consider returning a projection (DTO) instead of entity objects if only a subset of fields is required.
    /// </remarks>
    /// <param name="suiteId">The identifier of the test suite whose compositions should be retrieved.</param>
    /// <param name="ct">Cancellation token to cancel the operation.</param>
    /// <returns>
    /// A collection of <see cref="TestSuiteComposition"/> instances, or <c>null</c> if none are found.
    /// </returns>
    public virtual async Task<ICollection<TestSuiteComposition>?> GetByTestSuiteId(long suiteId, CancellationToken ct = default)
    {
        _logger?.LogDebug("TestSuiteCompositionService::GetByTestSuiteId() start!");

        var compositions = await _context.TestSuiteCompositions
            .Where(v => v.TestSuiteId == suiteId)
            .Include(v => v.TestCaseVersion)
                .ThenInclude(_ => _.TestLevel)
            .Include(v => v.TestSuite)
            .AsNoTracking()
            .ToListAsync(ct);

        return compositions;
    }

    /// <summary>
    /// Creates a composition that links a specific test case version to a test suite.
    /// If the referenced test case version does not exist this method returns <c>null</c>.
    /// If the composition already exists the existing composition information is returned,
    /// otherwise a new composition record is created and returned.
    /// </summary>
    /// <param name="request">A <see cref="CreateTestSuiteCompositionRequest"/> containing the target test suite id, test case id and version number.</param>
    /// <param name="ct">Cancellation token to cancel the asynchronous operation.</param>
    /// <returns>
    /// A <see cref="CreateTestSuiteCompositionResponse"/> describing the existing or newly created composition,
    /// or <c>null</c> when the requested test case version cannot be found.
    /// </returns>
    public virtual async Task<CreateTestSuiteCompositionResponse?> CreateCompositionAsync(CreateTestSuiteCompositionRequest request, CancellationToken ct = default)
    {
        _logger?.LogDebug("TestSuiteCompositionService::CreateComposition() start!");

        var testCaseVersion = await _context.TestCaseVersions
            .AsNoTracking()
            .FirstOrDefaultAsync(version =>
                version.TestCaseId == request.TestCaseId &&
                version.VersionNumber == request.TestCaseVersionNumber,
                ct);
        if (testCaseVersion is null)
        {
            return null;
        }

        var testCaseVersionId = testCaseVersion.Id;
        var isExists = await _context.TestSuiteCompositions
            .AsNoTracking()
            .AnyAsync(_ => _.TestSuiteId == request.TestSuiteId && _.TestCaseVersionId == testCaseVersionId, ct);
        if (isExists)
        {
            var testSuiteComposition = await _context.TestSuiteCompositions
                .AsNoTracking()
                .FirstOrDefaultAsync(_ => _.TestSuiteId == request.TestSuiteId && _.TestCaseVersionId == testCaseVersionId, ct);
            return new CreateTestSuiteCompositionResponse
            {
                TestSuiteCompositionId = testSuiteComposition!.Id,
                TestSuiteId = testSuiteComposition.TestSuiteId,
                TestCaseId = request.TestCaseId,
                TestCaseVersionNumber = request.TestCaseVersionNumber
            };
        }
        else
        {
            var testSuiteComposition = new TestSuiteComposition
            {
                TestSuiteId = request.TestSuiteId,
                TestCaseVersionId = testCaseVersionId
            };
            _context.TestSuiteCompositions.Add(testSuiteComposition);

            await _context.SaveChangesAsync(ct);

            return new CreateTestSuiteCompositionResponse
            {
                TestSuiteCompositionId = testSuiteComposition.Id,
                TestSuiteId = testSuiteComposition.TestSuiteId,
                TestCaseId = request.TestCaseId,
                TestCaseVersionNumber = request.TestCaseVersionNumber
            };
        }
    }

    /// <summary>
    /// Creates multiple compositions that link specific test case versions to test suites.
    /// For each request, if the referenced test case version does not exist the request is skipped.
    /// If a composition already exists the existing composition information is included in the response;
    /// otherwise a new composition record is created.
    /// </summary>
    /// <param name="requests">Collection of <see cref="CreateTestSuiteCompositionRequest"/> specifying test suite, test case and version.</param>
    /// <param name="ct">Cancellation token to cancel the asynchronous operation.</param>
    /// <returns>Collection of <see cref="CreateTestSuiteCompositionResponse"/> describing existing or newly created compositions.</returns>
    public virtual async Task<ICollection<CreateTestSuiteCompositionResponse>> CreateCompositionsAsync(ICollection<CreateTestSuiteCompositionRequest> requests, CancellationToken ct = default)
    {
        _logger?.LogDebug("TestSuiteCompositionService::CreateComposition() start!");

        var responses = new List<CreateTestSuiteCompositionResponse>();

        foreach (var request in requests)
        {
            var testCaseVersion = await _context.TestCaseVersions
                .AsNoTracking()
                .FirstOrDefaultAsync(version =>
                    version.TestCaseId == request.TestCaseId &&
                    version.VersionNumber == request.TestCaseVersionNumber,
                    ct);
            if (testCaseVersion is null)
            {
                continue;
            }

            var testCaseVersionId = testCaseVersion.Id;
            var isExists = await _context.TestSuiteCompositions
                .AsNoTracking()
                .AnyAsync(_ => _.TestSuiteId == request.TestSuiteId && _.TestCaseVersionId == testCaseVersionId, ct);
            if (isExists)
            {
                var testSuiteComposition = await _context.TestSuiteCompositions
                    .AsNoTracking()
                    .FirstOrDefaultAsync(_ => _.TestSuiteId == request.TestSuiteId && _.TestCaseVersionId == testCaseVersionId, ct);
                var response = new CreateTestSuiteCompositionResponse
                {
                    TestSuiteCompositionId = testSuiteComposition!.Id,
                    TestSuiteId = testSuiteComposition.TestSuiteId,
                    TestCaseId = request.TestCaseId,
                    TestCaseVersionNumber = request.TestCaseVersionNumber
                };
                responses.Add(response);
            }
            else
            {
                var testSuiteComposition = new TestSuiteComposition
                {
                    TestSuiteId = request.TestSuiteId,
                    TestCaseVersionId = testCaseVersionId
                };
                _context.TestSuiteCompositions.Add(testSuiteComposition);

                await _context.SaveChangesAsync(ct);

                var response = new CreateTestSuiteCompositionResponse
                {
                    TestSuiteCompositionId = testSuiteComposition.Id,
                    TestSuiteId = testSuiteComposition.TestSuiteId,
                    TestCaseId = request.TestCaseId,
                    TestCaseVersionNumber = request.TestCaseVersionNumber
                };
                responses.Add(response);
            }
        }

        return responses;
    }
}
