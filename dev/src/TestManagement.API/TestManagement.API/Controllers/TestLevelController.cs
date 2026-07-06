using Microsoft.AspNetCore.Mvc;
using TestManagement.API.Features.TestLevel.Get;
using TestManagement.API.Models;
using TestManagement.API.Services;

namespace TestManagement.API.Controllers
{
    /// <summary>
    /// API controller for managing test levels.
    /// Provides endpoints to retrieve test level information.
    /// </summary>
    [ApiController]
    [Route("api/testlevels")]
    public class TestLevelController : Controller
    {
        /// <summary>
        /// Service for handling test level business logic and data operations.
        /// </summary>
        private readonly ITestLevelService _testLevelService;

        /// <summary>
        /// Logger instance for recording controller events and debugging information.
        /// </summary>
        private readonly ILogger<TestLevelController> _logger;

        /// <summary>
        /// Initializes a new instance of the TestLevelController class.
        /// </summary>
        /// <param name="logger">The logger instance for logging controller operations.</param>
        /// <param name="testLevelRepository">The test level service for data operations.</param>
        public TestLevelController(
            ILogger<TestLevelController> logger, 
            ITestLevelService testLevelRepository)
        {
            _logger = logger;
            _testLevelService = testLevelRepository;
        }

        /// <summary>
        /// Retrieves all test levels asynchronously.
        /// </summary>
        /// <returns>An IActionResult containing a list of all test levels with HTTP 200 OK status.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ICollection<GetTestLevelResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ICollection<GetTestLevelResponse>>> GetAllAsync(CancellationToken ct)
        {
            _logger.LogDebug("TestLevelController.GetAllTestLevels() start!");

            ICollection<GetTestLevelResponse> testLevels = await _testLevelService.GetAllAsync(ct);
            return Ok(testLevels);
        }
    }
}
