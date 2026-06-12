using Microsoft.AspNetCore.Mvc;
using TestManagement.API.Features.TestExecutions.Create;
using TestManagement.API.Features.TestExecutions.Get;
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
        /// <summary>
        /// Logger for recording diagnostic and operational messages for the controller.
        /// </summary>
        private readonly ILogger<TestExecutionController> _logger;

        /// <summary>
        /// Service responsible for handling test execution operations and business logic.
        /// </summary>
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
        /// Retrieves all test executions including their environments, items and test results.
        /// This is a GET endpoint that returns a collection of execution summary DTOs.
        /// </summary>
        /// <param name="ct">Cancellation token to cancel the request.</param>
        /// <returns>A list of <see cref="GetTestExecutionResponse"/> DTOs representing stored test executions.</returns>
        [HttpGet]
        public async Task<List<GetTestExecutionResponse>> GetAllAsync(CancellationToken ct = default)
        {
            _logger.LogDebug("TestExecutionController.GetAsync() start!");

            var response = await _testExecutionService.GetAsync(ct);
            return response;
        }

        /// <summary>
        /// Retrieves a single test execution by its identifier.
        /// Returns the execution summary if found; otherwise returns an empty DTO.
        /// </summary>
        /// <param name="id">The identifier of the test execution to retrieve.</param>
        /// <param name="ct">Cancellation token to cancel the request.</param>
        /// <returns>An instance of <see cref="GetTestExecutionResponse"/> for the requested execution, or an empty instance when not found.</returns>
        [HttpGet("{id}")]
        public async Task<GetTestExecutionResponse> GetByIdAsync(long id, CancellationToken ct = default)
        {
            _logger.LogDebug("TestExecutionController.GetAsync() start!");

            var response = await _testExecutionService.GetByIdAsync(id, ct);
            return response;
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
