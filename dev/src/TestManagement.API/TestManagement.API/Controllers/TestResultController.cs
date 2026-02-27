using Microsoft.AspNetCore.Mvc;
using TestManagement.API.Models;
using TestManagement.API.Models.Report.Xml;
using TestManagement.API.Services;

namespace TestManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestResultController : ControllerBase
    {
        private readonly TestResultService _testResultService;
        private readonly ILogger<TestResultController> _logger;

        public TestResultController(ILogger<TestResultController> logger, TestResultService testResultService)
        {
            _logger = logger;
            _testResultService = testResultService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTestResults()
        {
            _logger.LogDebug("TestResultController.GetAllTestResults() start!");

            var testResults = await _testResultService.GetAllAsync();
            return Ok(testResults);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogDebug("TestResultController.GetById() start!");

            var testResults = await _testResultService.GetByIdAsync(id);
            return Ok(testResults);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TestResult testResult)
        {
            _logger.LogDebug("TestResultController.Create() start!");

            await _testResultService.Create(testResult);

            return CreatedAtAction(nameof(GetById), new { id = testResult.Id }, testResult);
        }

        [HttpPost("Bulk")]
        public async Task<IActionResult> CreateBulk(List<TestResult> testResults)
        {
            _logger.LogDebug("TestResultController.CreateBulk() start!");

            await _testResultService.Create(testResults);

            return CreatedAtAction(nameof(GetById), new { id = testResults[0].Id }, testResults);
        }

        [HttpPost("report")]
        [Consumes("application/xml")]
        public async Task<IActionResult> CreateFromXml([FromBody] TestSuitesXml suites)
        {
            _logger.LogDebug("TestResultController.CreateFromXml() start!");

            // Convert and persist XML suites to TestResult entities using service
            await _testResultService.Create(suites);

            return Ok();
        }
    }
}
