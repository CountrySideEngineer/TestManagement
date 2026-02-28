using Microsoft.AspNetCore.Mvc;
using TestManagement.API.Models;
using TestManagement.API.Services;

namespace TestManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestRunController : ControllerBase
    {
        private readonly TestRunService _testRunService;
        private readonly ILogger<TestRunController> _logger;

        public TestRunController(ILogger<TestRunController> logger, TestRunService testRunService)
        {
            _logger = logger;
            _testRunService = testRunService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTestRunsAsync()
        {
            _logger.LogDebug("TestRunController.GetAllTestRuns() start!");

            var testRuns = await _testRunService.GetAllAsync();
            return Ok(testRuns);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            _logger.LogDebug("TestRunController.GetById() start!");

            var testRun = await _testRunService.GetByIdAsync(id);
            if (testRun == null)
            {
                return NotFound();
            }
            return Ok(testRun);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(TestRun testRun)
        {
            _logger.LogDebug("TestRunController.Create() start!");

            await _testRunService.CreateAsync(testRun);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = testRun.Id }, testRun);
        }

        [HttpPost("Bulk")]
        public async Task<IActionResult> CreateBulkAsync(List<TestRun> testRuns)
        {
            _logger.LogDebug("TestRunController.CreateBulk() start!");

            await _testRunService.CreateAsync(testRuns);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = testRuns[0].Id }, testRuns);
        }
    }
}
