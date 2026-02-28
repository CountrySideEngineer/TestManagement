using Microsoft.AspNetCore.Mvc;
using TestManagement.API.Models;
using TestManagement.API.Services;

namespace TestManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestCaseController : ControllerBase
    {
        private readonly TestCaseService _testCaseService;
        private readonly ILogger<TestCaseController> _logger;

        public TestCaseController(ILogger<TestCaseController> logger, TestCaseService testCaseService)
        {
            _logger = logger;
            _testCaseService = testCaseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTestCasesAsync()
        {
            _logger.LogDebug("TestCaseController.GetAllTestCases() start!");

            var testCases = await _testCaseService.GetAllAsync();
            return Ok(testCases);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            _logger.LogDebug("TestCaseController.GetById() start!");

            var testCases = await _testCaseService.GetByIdAsync(id);
            return Ok(testCases);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(TestCase testCase)
        {
            _logger.LogDebug("TestCaseController.Create() start!");

            if (null != testCase)
            {
                await _testCaseService.CreateAsync(testCase);
                return CreatedAtAction(nameof(GetByIdAsync), new { id = testCase.Id }, testCase);
            }
            else
            {
                return BadRequest("TestCase cannot be null.");
            }

        }

        [HttpPost("Bulk")]
        public async Task<IActionResult> CreateBulkAsync(List<TestCase> testCases)
        {
            _logger.LogDebug("TestCaseController.CreateBulk() start!");

            await _testCaseService.CreateAsync(testCases);

            return CreatedAtAction(nameof(GetByIdAsync), new { id = testCases[0].Id }, testCases);
        }
    }
}
