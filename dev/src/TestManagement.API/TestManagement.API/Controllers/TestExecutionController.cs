using Microsoft.AspNetCore.Mvc;
using TestManagement.API.Features.TestExecutions.Create;
using TestManagement.API.Features.TestExecutions.Get;
using TestManagement.API.Features.TestExecutions.Update;
using TestManagement.API.Services;

namespace TestManagement.API.Controllers
{
    /// <summary>
    /// API controller that exposes endpoints for managing test executions.
    /// </summary>
    [ApiController]
    [Route("api/testexecutions")]
    public class TestExecutionController : Controller
    {
        /// <summary>
        /// Logger for recording diagnostic and operational messages for the controller.
        /// </summary>
        private readonly ILogger<TestExecutionController> _logger;

        /// <summary>
        /// Service responsible for handling test execution operations and business logic.
        /// </summary>
        private readonly ITestExecutionService _testExecutionService;

        /// <summary>
        /// Constructs a new instance of <see cref="TestExecutionController"/>.
        /// </summary>
        /// <param name="logger">Logger instance for the controller.</param>
        /// <param name="testExecutionService">Service that handles test execution operations.</param>
        public TestExecutionController(
            ILogger<TestExecutionController> logger, 
            ITestExecutionService testExecutionService
            )
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
        [ProducesResponseType(typeof(ICollection<GetTestExecutionResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ICollection<GetTestExecutionResponse>>> GetAllAsync(CancellationToken ct = default)
        {
            _logger.LogDebug("TestExecutionController.GetAsync() start!");

            ICollection<GetTestExecutionResponse> responses = await _testExecutionService.GetAsync(ct);
            return Ok(responses);
        }

        /// <summary>
        /// Retrieves a single test execution by its identifier.
        /// Returns the execution summary if found; otherwise returns an empty DTO.
        /// </summary>
        /// <param name="id">The identifier of the test execution to retrieve.</param>
        /// <param name="ct">Cancellation token to cancel the request.</param>
        /// <returns>An instance of <see cref="GetTestExecutionResponse"/> for the requested execution, or an empty instance when not found.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetTestExecutionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GetTestExecutionResponse>> GetByIdAsync(long id, CancellationToken ct = default)
        {
            _logger.LogDebug("TestExecutionController.GetAsync() start!");

            GetTestExecutionResponse response = await _testExecutionService.GetByIdAsync(id, ct);
            return Ok(response);
        }

        /// <summary>
        /// Creates a new test execution record from the provided request DTO.
        /// This is a POST endpoint that returns the created execution details.
        /// </summary>
        /// <param name="request">The request containing execution metadata and test cases.</param>
        /// <param name="ct">Cancellation token to cancel the request.</param>
        /// <returns>The created <see cref="CreateTestExecutionResponse"/>.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(CreateTestExecutionResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CreateTestExecutionResponse>> CreateAsync([FromBody] CreateTestExecutionRequest request, CancellationToken ct = default)
        {
            _logger.LogDebug("TestExecutionController.CreateAsync() start!");

            CreateTestExecutionResponse response = await _testExecutionService.CreateAsync(request, ct);
            return CreatedAtAction(
                nameof(GetByIdAsync),
                new { id = response.TestExecutionId },
                response
                );
        }

        /// <summary>
        /// Updates an existing test execution by adding a new execution item.
        /// This is a PUT endpoint that returns the updated execution details.
        /// </summary>
        /// <param name="request">The request containing execution metadata and test cases to add.</param>
        /// <param name="ct">Cancellation token to cancel the request.</param>
        /// <returns>The updated <see cref="UpdateTestExecutionResponse"/>.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UpdateTestExecutionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UpdateTestExecutionResponse>> UpdateAsync(long id, [FromBody] UpdateTestExecutionRequest request, CancellationToken ct = default)
        {
            _logger.LogDebug("TestExecutionController.UpdateAsync() start!");

            UpdateTestExecutionResponse response = await _testExecutionService.UpdateAsync(request, ct);
            return Ok(response);
        }
    }
}
