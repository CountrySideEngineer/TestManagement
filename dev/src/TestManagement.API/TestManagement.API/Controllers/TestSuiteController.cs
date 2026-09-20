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
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(GetTestSuiteResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ActionName(nameof(GetByIdAsync))]
    public async Task<ActionResult<GetTestSuiteResponse>> GetByIdAsync(long id, CancellationToken ct)
    {
        _logger?.LogDebug("TestSuiteController.GetTestSuiteById() start!");
        GetTestSuiteResponse testSuite = await _testSuiteFacadeService.GetByIdAsync(id, ct);
        return Ok(testSuite);
    }

    /// <summary>
    /// HTTP GET endpoint to retrieve a test suite and its composition (test case summaries).
    /// </summary>
    /// <remarks>
    /// - Route: GET /{id:long}/composition
    /// - Returns 200 with <see cref="GetTestSuiteResponse"/> on success.
    /// - Returns 404/500 as appropriate (handled by higher-level middleware or service layer exceptions).
    /// - This action delegates retrieval to <see cref="TestSuiteFacadeService.GetByIdWithTestCasesAsync(long, CancellationToken)"/>.
    /// </remarks>
    /// <param name="id">The id of the test suite to retrieve.</param>
    /// <param name="ct">Cancellation token to cancel the request.</param>
    /// <returns>
    /// An <see cref="ActionResult{GetTestSuiteResponse}"/> containing the suite and its test case summaries.
    /// </returns>
    [HttpGet("{id:long}/composition")]
    [ProducesResponseType(typeof(GetTestSuiteWithTestCaseResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ActionName(nameof(GetByIdWithTestCasesAsync))]
    public async Task<ActionResult<GetTestSuiteWithTestCaseResponse>> GetByIdWithTestCasesAsync(long id, CancellationToken ct)
    {
        _logger?.LogDebug("TestSuiteController.GetTestSuiteByIdWithTestCases() start!");

        GetTestSuiteWithTestCaseResponse testSuite = await _testSuiteFacadeService.GetByIdWithTestCasesAsync(id, ct);
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

    [HttpPost("{id:long}/composition")]
    [ProducesResponseType(typeof(CreateTestSuiteCompositionResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    public async Task<ActionResult<CreateTestSuiteCompositionResponse>> CreateCompositionAsync(long id, TestSuiteCompositionCreateRequest request, CancellationToken ct = default)
    {
        _logger?.LogDebug("TestSuiteController.CreateTestSuiteComposition() start!");

        var createRequest = new CreateTestSuiteCompositionRequest
        {
            TestSuiteId = id,
            TestCaseId = request.TestCaseId,
            TestCaseVersionNumber = request.TestCaseVersionNumber
        };

        CreateTestSuiteCompositionResponse createdTestSuiteComposition = await _testSuiteFacadeService.CreateCompositionAsync(createRequest, ct);

        var response = new TestSuiteCompositionCreateResponse()
        {
            TestSuiteCompositionId = createdTestSuiteComposition.TestSuiteCompositionId,
            TestSuiteId = createdTestSuiteComposition.TestSuiteId,
            TestCaseId = createdTestSuiteComposition.TestCaseId,
            TestCaseVersionNumber = createdTestSuiteComposition.TestCaseVersionNumber
        };

        return CreatedAtAction(nameof(GetByIdAsync), new { id = response.TestSuiteCompositionId}, response);
    }

    /// <summary>
    /// Creates multiple compositions that link test case versions to test suites.
    /// Delegates processing to the <see cref="ITestSuiteFacadeService"/> and returns
    /// a 201 Created response containing the collection of created or existing compositions.
    /// If the facade service produces no results an empty collection is returned in the response body.
    /// The Location header is set using the first composition id in the returned collection when available.
    /// </summary>
    /// <param name="requests">A collection of <see cref="CreateTestSuiteCompositionRequest"/> describing the compositions to create.</param>
    /// <param name="ct">A <see cref="CancellationToken"/> to cancel the operation.</param>
    /// <returns>
    /// An <see cref="ActionResult{ICollection}"/> containing the created or existing
    /// <see cref="CreateTestSuiteCompositionResponse"/> objects and a 201 Created status.
    /// </returns>
    [HttpPost("{id:long}/compositions")]
    [ProducesResponseType(typeof(ICollection<CreateTestSuiteCompositionResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    public async Task<ActionResult<ICollection<CreateTestSuiteCompositionResponse>>> CreateCompositionsAsync(
        long id, 
        ICollection<TestSuiteCompositionCreateRequest> requests, 
        CancellationToken ct = default
        )
    {
        _logger?.LogDebug("TestSuiteController.CreateTestSuiteComposition() start!");

        var createRequests = requests.Select(r => new CreateTestSuiteCompositionRequest
            {
                TestSuiteId = id,
                TestCaseId = r.TestCaseId,
                TestCaseVersionNumber = r.TestCaseVersionNumber
            })
            .ToList();

        ICollection<CreateTestSuiteCompositionResponse> createdTestSuiteCompositions = await _testSuiteFacadeService.CreateCompositionsAsync(createRequests, ct);

        var responses = new TestSuiteCompositionCreateResponse
        {
            TestSuiteCompositionId = createdTestSuiteCompositions.FirstOrDefault()?.TestSuiteCompositionId ?? 0,
            TestSuiteId = createdTestSuiteCompositions.FirstOrDefault()?.TestSuiteId ?? 0,
            TestCaseId = createdTestSuiteCompositions.FirstOrDefault()?.TestCaseId ?? 0,
            TestCaseVersionNumber = createdTestSuiteCompositions.FirstOrDefault()?.TestCaseVersionNumber ?? 0
        };

        return Ok(responses);
    }
}
