using Microsoft.AspNetCore.Mvc;
using TestManagement.API.Features.testExecutions.Create;
using TestManagement.API.Features.TestExecutions.Create;
using TestManagement.API.Services;

namespace TestManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    /// <summary>
    /// API controller that exposes endpoints for managing test executions.
    /// </summary>
    public class TestExecutionController : ControllerBase
    {
        private readonly ILogger<TestExecutionController> _logger;

        private readonly TestExecutionService _testExecutionService;

        /// <summary>
        /// Constructs a new instance of <see cref="TestExecutionController"/>.
        /// </summary>
        /// <param name="logger">Logger instance for the controller.</param>
        /// <param name="testExecutionService">Service that handles test execution operations.</param>
        public TestExecutionController(ILogger<TestExecutionController> logger, TestExecutionService testExecutionService)
        {
            _logger = logger;
            _testExecutionService = testExecutionService;
        }

        /// <summary>
        /// Creates a new test execution record from the provided request DTO.
        /// This is a POST endpoint that returns the created execution details.
        /// </summary>
        /// <param name="request">The request containing execution metadata and test cases.</param>
        /// <returns>The created <see cref="CreateTestExecutionResponse"/>.</returns>
        [HttpPost]
        public async Task<CreateTestExecutionResponse> CreateAsync([FromBody] CreateTestExecutionRequest request)
        {
            _logger.LogDebug("TestExecutionController.CreateAsync() start!");

            var response = await _testExecutionService.CreateAsync(request);
            return response;
        }
    }
}
