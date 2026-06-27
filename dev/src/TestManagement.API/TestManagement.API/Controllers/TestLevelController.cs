using Microsoft.AspNetCore.Mvc;
using TestManagement.API.Models;
using TestManagement.API.Services;

namespace TestManagement.API.Controllers
{
    /// <summary>
    /// API controller for managing test levels.
    /// Provides endpoints to retrieve test level information.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TestLevelController : ControllerBase
    {
        /// <summary>
        /// Service for handling test level business logic and data operations.
        /// </summary>
        private readonly TestLevelService _testLevelService;

        /// <summary>
        /// Logger instance for recording controller events and debugging information.
        /// </summary>
        private readonly ILogger<TestLevelController> _logger;

        /// <summary>
        /// Initializes a new instance of the TestLevelController class.
        /// </summary>
        /// <param name="logger">The logger instance for logging controller operations.</param>
        /// <param name="testLevelRepository">The test level service for data operations.</param>
        public TestLevelController(ILogger<TestLevelController> logger, TestLevelService testLevelRepository)
        {
            _logger = logger;
            _testLevelService = testLevelRepository;
        }

        /// <summary>
        /// Retrieves all test levels asynchronously.
        /// </summary>
        /// <returns>An IActionResult containing a list of all test levels with HTTP 200 OK status.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllTestLevelsAsync()
        {
            _logger.LogDebug("TestLevelController.GetAllTestLevels() start!");

            ICollection<TestLevel> testLevels = await _testLevelService.GetAllAsync();
            return Ok(testLevels);
        }
    }
}
