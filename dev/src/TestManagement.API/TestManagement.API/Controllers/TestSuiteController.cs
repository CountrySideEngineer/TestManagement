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
    private readonly ITestSuiteFacadeService _testSuiteFacadeService;

    /// <summary>
    /// Optional logger for the controller.
    /// </summary>
    private readonly ILogger<TestSuiteController>? _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestSuiteController"/> class.
    /// </summary>
    /// <param name="logger">Optional logger for the controller.</param>
    /// <param name="testFacadeService">Service that encapsulates business logic for test suites.</param>
    public TestSuiteController(
        ILogger<TestSuiteController>? logger,
        ITestSuiteFacadeService testFacadeService)
    {
        _logger = logger;
        _testSuiteFacadeService = testFacadeService;
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
        ICollection<GetTestSuiteResponse> testSuites = await _testSuiteFacadeService.GetAllAsync(ct);
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
        GetTestSuiteResponse testSuite = await _testSuiteFacadeService.GetByIdAsync(id, ct);
        return Ok(testSuite);
    }

    /// <summary>
    /// Creates a new test suite resource.
    /// The request is forwarded to the <see cref="ITestSuiteService"/> which persists the entity.
    /// On success this endpoint returns a 201 Created response with the location of the created resource.
    /// </summary>
    /// <param name="request">Request containing the properties for the new test suite (name, description).</param>
    /// <param name="ct">Cancellation token to cancel the operation.</param>
    /// <returns>
    /// An <see cref="ActionResult{CreateTestSuiteResponse}"/> containing the created test suite details.
    /// Returns 201 Created when the resource is successfully created.
    /// </returns>
    [HttpPost]
    [ProducesResponseType(typeof(CreateTestSuiteResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    public async Task<ActionResult<CreateTestSuiteResponse>> CreateAsync(CreateTestSuiteRequest request, CancellationToken ct)
    {
        _logger?.LogDebug("TestSuiteController.CreateTestSuite() start!");

        CreateTestSuiteResponse createdTestSuite = await _testSuiteFacadeService.CreateAsync(request, ct);
        return CreatedAtAction(nameof(GetByIdAsync), new { id = createdTestSuite.Id }, createdTestSuite);
    }

    [HttpPost("composition")]
    [ProducesResponseType(typeof(CreateTestSuiteCompositionResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    public async Task<ActionResult<CreateTestSuiteCompositionResponse>> CreateCompositionAsync(CreateTestSuiteCompositionRequest request, CancellationToken ct = default)
    {
        _logger?.LogDebug("TestSuiteController.CreateTestSuiteComposition() start!");

        CreateTestSuiteCompositionResponse createdTestSuiteComposition = await _testSuiteFacadeService.CreateCompositionAsync(request, ct);

        return CreatedAtAction(nameof(GetByIdAsync), new { id = createdTestSuiteComposition.TestSuiteCompositionId}, createdTestSuiteComposition);
    }
}
