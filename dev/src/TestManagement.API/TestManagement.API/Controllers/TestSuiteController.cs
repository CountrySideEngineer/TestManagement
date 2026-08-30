using Microsoft.AspNetCore.Mvc;
using TestManagement.API.Features.TestSuite;
using TestManagement.API.Services;

namespace TestManagement.API.Controllers;

[ApiController]
[Route("api/testsuites")]
public class TestSuiteController : Controller
{
    private readonly ITestSuiteService _testSuiteService;

    private readonly ILogger<TestSuiteController>? _logger;

    public TestSuiteController(
        ILogger<TestSuiteController>? logger,
        ITestSuiteService testSuiteService)
    {
        _logger = logger;
        _testSuiteService = testSuiteService;
    }

    public async Task<ActionResult<ICollection<GetTestSuiteResponse>>> GetAllAsync(CancellationToken ct)
    {
        _logger?.LogDebug("TestSuiteController.GetAllTestSuites() start!");
        ICollection<GetTestSuiteResponse> testSuites = await _testSuiteService.GetAllAsync(ct);
        return Ok(testSuites);
    }   
}
