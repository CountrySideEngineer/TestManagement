using Microsoft.AspNetCore.Mvc;
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
}
