using Microsoft.AspNetCore.Mvc;
using TestManagement.API.Services;

namespace TestManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestLevelController : ControllerBase
    {
        private readonly TestLevelService _testLevelService;
        private readonly ILogger<TestLevelController> _logger;

        public TestLevelController(ILogger<TestLevelController> logger, TestLevelService testLevelRepository)
        {
            _logger = logger;
            _testLevelService = testLevelRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTestLevelsAsync()
        {
            _logger.LogDebug("TestLevelController.GetAllTestLevels() start!");

            var testLevels = await _testLevelService.GetAllAsync();
            return Ok(testLevels);
        }
    }
}
