using Microsoft.AspNetCore.Mvc;
using TestManagement.API.Services;

namespace TestManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestLevelControllers : ControllerBase
    {
        private readonly TestLevelService _testLevelService;

        public TestLevelControllers(TestLevelService testLevelRepository)
        {
            _testLevelService = testLevelRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTestLevels()
        {
            var testLevels = await _testLevelService.GetAllAsync();
            return Ok(testLevels);
        }
    }
}
