using Microsoft.AspNetCore.Mvc;
using TestManagement.API.Models;
using TestManagement.API.Models.Report.Xml;
using TestManagement.API.Models.Requests;
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
        public async Task<IActionResult> GetAllTestResultsAsync()
        {
            _logger.LogDebug("TestResultController.GetAllTestResults() start!");

            var testResults = await _testResultService.GetAllAsync();
            return Ok(testResults);
        }

        [HttpGet("{id}", Name = "GetByIdAsync")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            _logger.LogDebug("TestResultController.GetById() start!");

            var testResults = await _testResultService.GetByIdAsync(id);
            return Ok(testResults);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateTestResultRequest request)
        {
            _logger.LogDebug("TestResultController.Create() start!");

            // Map request DTO to domain model
            var testResult = new TestResult
            {
                TestCaseVersionId = request.TestCaseVersionId,
                TestExecutionItemId = request.TestExecutionItemId,
                StatusId = request.StatusId,
                ActualResult = request.ActualResult ?? string.Empty,
                Message = request.Message,
                ExecutedAt = request.ExecutedAt ?? DateTime.UtcNow
            };

            await _testResultService.CreateAsync(testResult);

            var resultAction = CreatedAtRoute(nameof(GetByIdAsync), new { id = testResult.Id }, testResult);

            return resultAction;
        }

        [HttpPost("Bulk")]
        public async Task<IActionResult> CreateBulkAsync(List<TestResult> testResults)
        {
            _logger.LogDebug("TestResultController.CreateBulk() start!");

            await _testResultService.CreateAsync(testResults);

            return CreatedAtAction(nameof(GetByIdAsync), new { id = testResults[0].Id }, testResults);
        }

        [HttpPost("report")]
        [Consumes("application/xml")]
        public async Task<IActionResult> CreateFromXmlAsync([FromBody] TestSuitesXml suites)
        {
            _logger.LogDebug("TestResultController.CreateFromXml() start!");

            // Convert and persist XML suites to TestResult entities using service
            await _testResultService.CreateAsync(suites);

            return Ok();
        }
    }
}
