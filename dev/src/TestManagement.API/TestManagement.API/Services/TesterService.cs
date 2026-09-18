using Microsoft.EntityFrameworkCore;
using TestManagement.API.Data;
using TestManagement.API.Features.Tester.Create;
using TestManagement.API.Features.Tester.Get;
using TestManagement.API.Models;

namespace TestManagement.API.Services;

public class TesterService : ITesterService
{
    private readonly TestManagementDbContext _context;

    private readonly ILogger<TesterService>? _logger;

    public TesterService(
        TestManagementDbContext context,
        ILogger<TesterService>? logger
        )
    {
        _context = context;
        _logger = logger;
    }

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
