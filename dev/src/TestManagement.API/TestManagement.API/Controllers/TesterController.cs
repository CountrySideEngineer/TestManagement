using Microsoft.AspNetCore.Mvc;
using TestManagement.API.Features.Tester.Create;
using TestManagement.API.Features.Tester.Get;
using TestManagement.API.Services;

namespace TestManagement.API.Controllers;

[ApiController]
[Route("api/testers")]
public class TesterController : Controller
{
    private readonly ITesterService _testerService;

    private readonly ILogger<TesterController> _logger;

    public TesterController(
        ITesterService testerService,
        ILogger<TesterController> logger)
    {
        _testerService = testerService;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves all testers.
    /// </summary>
    /// <param name="ct">Cancellation token to cancel the operation.</param>
    /// <returns>A collection of tester response objects.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ICollection<TesterGetResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ICollection<TesterGetResponse>>> GetAllAsync(
        CancellationToken ct)
    {
        _logger.LogDebug("TesterController.GetAllAsync() start!");

        ICollection<GetTesterResponse> testers =
            await _testerService.GetAllAsync(ct);

        ICollection<TesterGetResponse> responses =
            testers.Select(t => new TesterGetResponse
            {
                Id = t.Id,
                Name = t.Name,
                Email = t.Email
            })
            .ToList();

        return Ok(responses);
    }

    /// <summary>
    /// Creates a new tester.
    /// </summary>
    /// <param name="request">Request containing the tester's name and email address.</param>
    /// <param name="ct">Cancellation token to cancel the operation.</param>
    /// <returns>The created tester.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(TesterCreateResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<TesterCreateResponse>> CreateAsync(
        [FromBody] TesterCreateRequest request,
        CancellationToken ct)
    {
        _logger.LogDebug("TesterController.CreateAsync() start!");

        CreateTesterRequest createRequest = new()
        {
            Name = request.Name,
            Email = request.Email
        };

        CreateTesterResponse createdTester =
            await _testerService.CreateAsync(createRequest, ct);

        TesterCreateResponse response = new()
        {
            Id = createdTester.Id,
            Name = createdTester.Name,
            Email = createdTester.Email
        };

        return StatusCode(StatusCodes.Status201Created, response);
    }

    /// <summary>
    /// Retrieves a tester by its identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the tester.</param>
    /// <param name="ct">Cancellation token to cancel the operation.</param>
    /// <returns>The tester response.</returns>
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(TesterGetResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ActionName(nameof(GetByIdAsync))]
    public async Task<ActionResult<TesterGetResponse>> GetByIdAsync(
        long id,
        CancellationToken ct)
    {
        _logger.LogDebug("TesterController.GetByIdAsync() start. Id: {Id}", id);

        GetTesterResponse tester =
            await _testerService.GetByIdAsync(id, ct);

        TesterGetResponse response = new()
        {
            Id = tester.Id,
            Name = tester.Name,
            Email = tester.Email
        };

        return Ok(response);
    }
}
