using Microsoft.AspNetCore.Mvc;
using TestManagement.API.Features.TestSuite;
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
}
