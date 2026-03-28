using Microsoft.AspNetCore.Mvc;
using TestManagement.API.Features.testExecutions.Create;
using TestManagement.API.Features.TestExecutions.Create;
using TestManagement.API.Services;

namespace TestManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestExecutionController : ControllerBase
    {
        private readonly ILogger<TestExecutionController> _logger;

        private readonly TestExecutionService _testExecutionService;

        public TestExecutionController(ILogger<TestExecutionController> logger, TestExecutionService testExecutionService)
        {
            _logger = logger;
            _testExecutionService = testExecutionService;
        }

        [HttpPost]
        public async Task<CreateTestExecutionResponse> CreateAsync([FromBody] CreateTestExecutionRequest request)
        {
            _logger.LogDebug("TestExecutionController.CreateAsync() start!");

            var response = await _testExecutionService.CreateAsync(request);
            return response;
        }
    }
}
