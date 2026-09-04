using Microsoft.AspNetCore.Mvc;
using TestManagement.API.Features.TestSuite.Create;
using TestManagement.API.Features.TestSuite.Get;
using TestManagement.API.Services;

namespace TestManagement.API.Controllers;

/// <summary>
/// Controller for managing test suites.
/// Provides endpoints to retrieve and manage test suite resources.
/// </summary>
[ApiController]
[Route("api/testsuites")]
public class TestSuiteController : Controller
{
    /// <summary>
    /// Service that encapsulates business logic for test suites.
    /// </summary>
    private readonly ITestSuiteService _testSuiteService;

    /// <summary>
    /// Optional logger for the controller.
    /// </summary>
    private readonly ILogger<TestSuiteController>? _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestSuiteController"/> class.
    /// </summary>
    /// <param name="logger">Optional logger for the controller.</param>
    /// <param name="testSuiteService">Service that encapsulates business logic for test suites.</param>
    public TestSuiteController(
        ILogger<TestSuiteController>? logger,
        ITestSuiteService testSuiteService)
    {
        _logger = logger;
        _testSuiteService = testSuiteService;
    }

    /// <summary>
    /// Retrieves all test suites.
    /// </summary>
    /// <param name="ct">Cancellation token to cancel the operation.</param>
    /// <returns>ActionResult containing a collection of <see cref="GetTestSuiteResponse"/>.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ICollection<GetTestSuiteResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ICollection<GetTestSuiteResponse>>> GetAllAsync(CancellationToken ct)
    {
        _logger?.LogDebug("TestSuiteController.GetAllTestSuites() start!");
        ICollection<GetTestSuiteResponse> testSuites = await _testSuiteService.GetAllAsync(ct);
        return Ok(testSuites);
    }


    /// <summary>
    /// Retrieves a single test suite by its unique identifier.
    /// Returns a 200 OK with the test suite when found, or 404 Not Found when no matching test suite exists.
    /// </summary>
    /// <param name="id">The unique identifier of the test suite to retrieve.</param>
    /// <param name="ct">Cancellation token to cancel the operation.</param>
    /// <returns>
    /// An <see cref="ActionResult{GetTestSuiteResponse}"/> containing the test suite when found,
    /// or a NotFound result when the test suite does not exist.
    /// </returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GetTestSuiteResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GetTestSuiteResponse>> GetByIdAsync(long id, CancellationToken ct)
    {
        _logger?.LogDebug("TestSuiteController.GetTestSuiteById() start!");
        GetTestSuiteResponse testSuite = await _testSuiteService.GetByIdAsync(id, ct);
        return Ok(testSuite);
    }

    public async Task<ActionResult<GetTestSuiteResponse>> CreateAsync(CreateTestSuiteRequest request, CancellationToken ct)
    {
        _logger?.LogDebug("TestSuiteController.CreateTestSuite() start!");

        CreateTestSuiteResponse createdTestSuite = await _testSuiteService.CreateAsync(request, ct);
        return CreatedAtAction(nameof(GetByIdAsync), new { id = createdTestSuite.Id }, createdTestSuite);
    }
}
