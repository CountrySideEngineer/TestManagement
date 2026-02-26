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
        public async Task<IActionResult> GetAllTestCases()
        {
            _logger.LogInformation("TestCaseController.GetAllTestCases() start!");

            var testCases = await _testCaseService.GetAllAsync();
            return Ok(testCases);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("TestCaseController.GetById() start!");

            var testCases = await _testCaseService.GetByIdAsync(id);
            return Ok(testCases);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TestCase testCase)
        {
            _logger.LogInformation("TestCaseController.Create() start!");

            if (null != testCase)
            {
                await _testCaseService.Create(testCase);
                return CreatedAtAction(nameof(GetById), new { id = testCase.Id }, testCase);
            }
            else
            {
                return BadRequest("TestCase cannot be null.");
            }

        }

        [HttpPost("Bulk")]
        public async Task<IActionResult> CreateBulk(List<TestCase> testCases)
        {
            _logger.LogInformation("TestCaseController.CreateBulk() start!");

            await _testCaseService.Create(testCases);

            return CreatedAtAction(nameof(GetById), new { id = testCases[0].Id }, testCases);
        }
    }
}
