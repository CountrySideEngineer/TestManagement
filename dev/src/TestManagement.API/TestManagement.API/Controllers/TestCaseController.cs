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

        public TestCaseController(TestCaseService testCaseService)
        {
            _testCaseService = testCaseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTestCases()
        {
            var testCases = await _testCaseService.GetAllAsync();
            return Ok(testCases);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var testCases = await _testCaseService.GetByIdAsync(id);
            return Ok(testCases);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TestCase testCase)
        {
            await _testCaseService.Create(testCase);

            return CreatedAtAction(nameof(GetById), new { id = testCase.Id }, testCase);
        }

        [HttpPost("Bulk")]
        public async Task<IActionResult> CreateBulf(List<TestCase> testCases)
        {
            await _testCaseService.Create(testCases);

            return CreatedAtAction(nameof(GetById), new { id = testCases[0].Id }, testCases);
        }
    }
}
