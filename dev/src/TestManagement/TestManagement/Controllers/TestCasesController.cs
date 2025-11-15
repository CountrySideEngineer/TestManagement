using Microsoft.AspNetCore.Mvc;
using TestManagement.Data.Repositories;
using TestManagement.Model;

namespace TestManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestCasesController : ControllerBase
    {
        private readonly ITestCaseRepository _testCaseRepository;

        public TestCasesController(ITestCaseRepository testCaseRepository)
        {
            _testCaseRepository = testCaseRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var testCases = await _testCaseRepository.GetAllAsync();
            return Ok(testCases);
        }

        [HttpGet("suite/{testSuiteId}")]
        public async Task<IActionResult> GetByTestSuite(int testSuiteId)
        {
            var testCases = await _testCaseRepository.GetByTestSuiteIdAsync(testSuiteId);
            return Ok(testCases);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var testCase = await _testCaseRepository.GetByIdAsync(id);
            if (null == testCase)
            {
                return NotFound();
            }
            return Ok(testCase);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TestCase testCase)
        {
            await _testCaseRepository.AddAsync(testCase);
            return CreatedAtAction(nameof(GetById), new { id = testCase.Id }, testCase);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TestCase testCase)
        {
            if (id != testCase.Id)
            {
                return BadRequest();
            }
            await _testCaseRepository.UpdateAsync(testCase);
            return NoContent();
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _testCaseRepository.DeleteAsync(id);
            return NoContent();
        }   
    }
}
