using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using TestManagement.Data.Repositories;
using TestManagement.Model;

namespace TestManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestSuitesController : ControllerBase
    {
        private readonly ITestSuiteRepository _repository;

        public TestSuitesController(ITestSuiteRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var suites = await _repository.GetAllAsync();
            return Ok(suites);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var suite = await _repository.GetByIdAsync(id);
            if (null == suite)
            {
                return NotFound();
            }
            return Ok(suite);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TestSuite suite)
        {
            await _repository.AddAsync(suite);
            return CreatedAtAction(nameof(GetById), new { id = suite.Id }, suite);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TestSuite suite)
        {
            if (id != suite.Id)
            {
                return BadRequest();
            }
            await _repository.UpdateAsync(suite);
            return NoContent();
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}
