using Microsoft.EntityFrameworkCore;
using TestManagement.API.Data;
using TestManagement.API.Features.Tester.Create;
using TestManagement.API.Features.Tester.Get;
using TestManagement.API.Models;

namespace TestManagement.API.Services;

/// <summary>
/// Service for managing tester entities and providing related operations.
/// Implements CRUD operations for tester data and handles data persistence.
/// </summary>
public class TesterService : ITesterService
{
    /// <summary>
    /// Database context used to access and persist tester-related entities.
    /// </summary>
    private readonly TestManagementDbContext _context;

    /// <summary>
    /// Logger instance for recording diagnostic and trace information.
    /// </summary>
    private readonly ILogger<TesterService>? _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TesterService"/> class.
    /// </summary>
    /// <param name="context">The database context for data access.</param>
    /// <param name="logger">Optional logger for diagnostic logging.</param>
    public TesterService(
        TestManagementDbContext context,
        ILogger<TesterService>? logger
        )
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves all testers asynchronously with no tracking.
    /// </summary>
    /// <param name="ct">Cancellation token to cancel the operation.</param>
    /// <returns>A collection of <see cref="GetTesterResponse"/> objects representing all testers.</returns>
    public async Task<ICollection<GetTesterResponse>> GetAllAsync(CancellationToken ct)
    {
        _logger?.LogDebug("TesterService::GetAllAsync() start!");

        var testers = await _context.Testers
            .AsNoTracking()
            .ToListAsync(ct);

        var response = testers
            .Select(t => new GetTesterResponse
            {
                Id = t.Id,
                Name = t.Name,
                Email = t.Email
            })
            .ToList();

        _logger?.LogDebug("TesterService::GetAllAsync() finished. Returning {Count} testers.", response.Count);

        return response;
    }

    /// <summary>
    /// Retrieves a specific tester by its identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the tester to retrieve.</param>
    /// <param name="ct">Cancellation token to cancel the operation.</param>
    /// <returns>A <see cref="GetTesterResponse"/> object representing the tester.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no tester with the specified identifier is found.</exception>
    public async Task<GetTesterResponse> GetByIdAsync(long id, CancellationToken ct)
    {
        _logger?.LogDebug("TesterService::GetByIdAsync({Id}) start!", id);

        var tester = await _context.Testers
            .AsNoTracking()
            .Where(t => t.Id == id)
            .FirstAsync(ct);

        var response = new GetTesterResponse
        {
            Id = tester.Id,
            Name = tester.Name,
            Email = tester.Email
        };

        return response;
    }

    /// <summary>
    /// Creates a new tester asynchronously and persists it to the database.
    /// </summary>
    /// <param name="request">The <see cref="CreateTesterRequest"/> containing the tester's information to create.</param>
    /// <param name="ct">Cancellation token to cancel the operation.</param>
    /// <returns>A <see cref="CreateTesterResponse"/> containing the created tester's details including the generated identifier.</returns>
    /// <exception cref="Exception">Thrown when an error occurs during database persistence.</exception>
    public async Task<CreateTesterResponse> CreateAsync(CreateTesterRequest request, CancellationToken ct)
    {
        _logger?.LogDebug("TesterService::CreateAsync() start!");

        var newTester = new Tester
        {
            Name = request.Name,
            Email = request.Email
        };

        _context.Testers.Add(newTester);

        try
        {
            await _context.SaveChangesAsync(ct);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error occurred while saving new tester to the database.");
            throw;
        }

        var response = new CreateTesterResponse
        {
            Id = newTester.Id,
            Name = newTester.Name,
            Email = newTester.Email
        };

        _logger?.LogDebug("TesterService::CreateAsync() finished. Created tester with Id: {Id}", newTester.Id);

        return response;
    }
}
