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
        public async Task<IActionResult> GetAllTestRuns()
        {
            _logger.LogInformation("Getting all test runs");
            var testRuns = await _testRunService.GetAllAsync();
            return Ok(testRuns);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var testRun = await _testRunService.GetByIdAsync(id);
            if (testRun == null)
            {
                return NotFound();
            }
            return Ok(testRun);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TestRun testRun)
        {
            await _testRunService.Create(testRun);
            return CreatedAtAction(nameof(GetById), new { id = testRun.Id }, testRun);
        }

        [HttpPost("Bulk")]
        public async Task<IActionResult> CreateBulk(List<TestRun> testRuns)
        {
            await _testRunService.Create(testRuns);
            return CreatedAtAction(nameof(GetById), new { id = testRuns[0].Id }, testRuns);
        }
    }
}
