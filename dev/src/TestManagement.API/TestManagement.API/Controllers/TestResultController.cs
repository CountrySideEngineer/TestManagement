using Microsoft.AspNetCore.Mvc;
using TestManagement.API.Models;
using TestManagement.API.Services;

namespace TestManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestResultController : ControllerBase
    {
        private readonly TestResultService _testResultService;

        public TestResultController(TestResultService testResultService)
        {
            _testResultService = testResultService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTestResults()
        {
            var testResults = await _testResultService.GetAllAsync();
            return Ok(testResults);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var testResults = await _testResultService.GetByIdAsync(id);
            return Ok(testResults);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TestResult testResult)
        {
            await _testResultService.Create(testResult);

            return CreatedAtAction(nameof(GetById), new { id = testResult.Id }, testResult);
        }

        [HttpPost("Bulk")]
        public async Task<IActionResult> CreateBulk(List<TestResult> testResults)
        {
            await _testResultService.Create(testResults);

            return CreatedAtAction(nameof(GetById), new { id = testResults[0].Id }, testResults);
        }
    }
}
