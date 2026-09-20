using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TestManagement.API.Data;
using TestManagement.API.Features.TestSuite.Create;
using TestManagement.API.Features.TestSuite.Get;
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

        var suites = await _context.TestSuites
            .AsNoTracking()
            .Select(ts => new GetTestSuiteResponse
            {
                Id = ts.Id,
                Name = ts.Name,
                Description = ts.Description
            })
            .ToListAsync(ct);

        return suites;
    }

    /// <summary>
    /// Retrieve a single test suite by its primary key identifier. Returns null
    /// when no matching record exists. Related suite versions (cases) are included
    /// in the returned entity.
    /// </summary>
    /// <param name="id">Identifier of the test suite to retrieve.</param>
    /// <param name="ct">Cancellation token to cancel the operation.</param>
    /// <returns>The matching <see cref="GetTestSuiteResponse"/> if found; otherwise null.</returns>
    public async Task<GetTestSuiteResponse> GetByIdAsync(long id, CancellationToken ct = default)
    {
        _logger?.LogDebug("TestSuiteService::GetByIdAsync() start!");

        var response = await _context.TestSuites
            .Where(ts => ts.Id == id)
            .AsNoTracking()
            .Select(ts => new GetTestSuiteResponse
            {
                Id = ts.Id,
                Name = ts.Name,
                Description = ts.Description,
            })
            .FirstOrDefaultAsync(ct);

        if (response is null)
        {
            return new GetTestSuiteResponse();
        }
        else
        {
            return response;
        }
    }

    /// <summary>
    /// Create a new test suite record in the database.
    /// This method validates that no other test suite exists with the same name,
    /// constructs a new <see cref="TestSuite"/> entity, persists it and returns
    /// a response containing the created entity's key information.
    /// </summary>
    /// <param name="request">The request containing properties for the new test suite (name, description).</param>
    /// <param name="ct">Cancellation token to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="CreateTestSuiteResponse"/> with the created test suite's identifier and details.</returns>
    /// <exception cref="Exception">Thrown when a test suite with the same name already exists or when saving to the database fails.</exception>
    public async Task<CreateTestSuiteResponse> CreateAsync(CreateTestSuiteRequest request, CancellationToken ct = default)
    {
        _logger?.LogDebug("TestSuiteService::GetByIdAsync() start!");

        var isExists = await _context.TestSuites.AnyAsync(ts => ts.Name == request.Name);
        if (isExists)
        {
            throw new Exception($"A test suite with the name '{request.Name}' already exists.");
        }
        var newTestSuite = new TestSuite()
        {
            Name = request.Name,
            Description = request.Description,
        };

        _context.TestSuites.Add(newTestSuite);
        try
        {
            await _context.SaveChangesAsync(ct);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "An error occurred while creating a new test suite.");

            throw new Exception("An error occurred while creating a new test suite.", ex);
        }
        var response = new CreateTestSuiteResponse()
        {
            Id = newTestSuite.Id,
            Name = newTestSuite.Name,
            Description = newTestSuite.Description,
        };
        return response;
    }
}
