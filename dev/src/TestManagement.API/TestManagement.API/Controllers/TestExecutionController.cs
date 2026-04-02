using Microsoft.AspNetCore.Mvc;
using TestManagement.API.Features.testExecutions.Create;
using TestManagement.API.Features.TestExecutions.Create;
using TestManagement.API.Features.TestExecutions.Update;
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
        /// <param name="ct">Cancellation token to cancel the request.</param>
        /// <returns>The created <see cref="CreateTestExecutionResponse"/>.</returns>
        [HttpPost]
        public async Task<CreateTestExecutionResponse> CreateAsync([FromBody] CreateTestExecutionRequest request, CancellationToken ct = default)
        {
            _logger.LogDebug("TestExecutionController.CreateAsync() start!");

            var response = await _testExecutionService.CreateAsync(request, ct);
            return response;
        }

        /// <summary>
        /// Updates an existing test execution by adding a new execution item.
        /// This is a PUT endpoint that returns the updated execution details.
        /// </summary>
        /// <param name="request">The request containing execution metadata and test cases to add.</param>
        /// <param name="ct">Cancellation token to cancel the request.</param>
        /// <returns>The updated <see cref="UpdateTestExecutionResponse"/>.</returns>
        [HttpPut]
        public async Task<UpdateTestExecutionResponse> UpdateAsync([FromBody] UpdateTestExecutionRequest request, CancellationToken ct = default)
        {
            _logger.LogDebug("TestExecutionController.UpdateAsync() start!");

            var response = await _testExecutionService.UpdateAsync(request, ct);
            return response;
        }
    }
}
