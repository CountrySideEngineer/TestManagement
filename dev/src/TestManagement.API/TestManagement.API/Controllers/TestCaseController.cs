using Microsoft.AspNetCore.Mvc;
using TestManagement.API.Features.TestCases.Create;
using TestManagement.API.Features.TestCases.Get;
using TestManagement.API.Features.TestCases.Update;
using TestManagement.API.Models;
using TestManagement.API.Services;

namespace TestManagement.API.Controllers
{
    /// <summary>
    /// API controller that exposes endpoints for managing test cases and their versions.
    /// </summary>
    [ApiController]
    [Route("api/testcases")]
    public class TestCaseController : Controller
    {
        /// <summary>
        /// Service layer instance that encapsulates business logic and data operations for test cases.
        /// The controller delegates use-case operations to this service.
        /// </summary>
        private readonly ITestCaseService _testCaseService;

        /// <summary>
        /// Logger instance used for diagnostic and audit logging within the controller.
        /// </summary>
        private readonly ILogger<TestCaseController> _logger;

        /// <summary>
        /// Creates a new instance of <see cref="TestCaseController"/>.
        /// </summary>
        /// <param name="logger">Logger instance used for diagnostic messages.</param>
        /// <param name="testCaseService">Service that encapsulates test case use-cases.</param>
        public TestCaseController(ILogger<TestCaseController> logger, ITestCaseService testCaseService)
        {
            _logger = logger;
            _testCaseService = testCaseService;
        }

        /// <summary>
        /// Returns all test case versions including their associated test level information.
        /// </summary>
        /// <returns>HTTP 200 with the collection of test case versions.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ICollection<GetTestCaseResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ICollection<GetTestCaseResponse>>> GetAllAsync(CancellationToken ct)
        {
            _logger.LogDebug("TestCaseController.GetAllAsync() start!");

            ICollection<GetTestCaseResponse> testCases = await _testCaseService.GetAllAsync(ct);
            return Ok(testCases);
        }
        /// <summary>
        /// Retrieves a test case by its identifier, including its versions and associated test level information.
        /// </summary>
        /// <param name="id">The identifier of the test case to retrieve.</param>
        /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>
        /// An <see cref="ActionResult{GetTestCaseResponse}"/> containing the requested <see cref="GetTestCaseResponse"/> when found;
        /// returns a NotFound result when the test case does not exist. May return an error status for server-side failures.
        /// </returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetTestCaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GetTestCaseResponse>> GetByIdAsync(long id, CancellationToken ct)
        {
            _logger.LogDebug("TestCaseController.GetByIdAsync() start!");

            GetTestCaseResponse testCase = await _testCaseService.GetByTestCaseIdAsync(id, ct);
            return Ok(testCase);
        }

        /// <summary>
        /// Returns the test case version that corresponds to the provided version id.
        /// </summary>
        /// <param name="versionId">Identifier (long) of the test case version to retrieve.</param>
        /// <returns>HTTP 200 with the matching <see cref="TestManagement.API.Models.TestCaseVersion"/> or 404 if not found.</returns>
        [HttpGet("versions/{versionId}")]
        [ProducesResponseType(typeof(TestCaseVersion), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TestCaseVersion>> GetByVersionIdAsync(long versionId, CancellationToken ct)
        {
            _logger.LogDebug("TestCaseController.GetByVersionIdAsync() start!");

            var testCaseVersion = await _testCaseService.GetByVersionIdAsync(versionId, ct);
            if (testCaseVersion == null)
            {
                return NotFound();
            }
            return Ok(testCaseVersion);
        }

        /// <summary>
        /// Creates a new test case with an initial version.
        /// </summary>
        /// <param name="request">Request payload containing code, name, description and test level id.</param>
        /// <returns>Response containing the created test case version details.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(CreateTestCaseResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CreateTestCaseResponse>> CreateAsync([FromBody] CreateTestCaseRequest request, CancellationToken ct)
        {
            _logger.LogDebug("TestCaseController.Create() start!");
            var response = await _testCaseService.CreateAsync(request, ct);

            return CreatedAtAction(
                nameof(GetByIdAsync),
                new { id = response.Id },
                response);
        }

        /// <summary>
        /// Attempts to create multiple test cases from the provided requests and returns per-request responses.
        /// </summary>
        /// <param name="requests">Collection of <see cref="CreateTestCaseRequest"/> objects to process.</param>
        /// <returns>
        /// A collection of <see cref="CreateTestCaseResponse"/> objects describing the result for each request.
        /// Successful entries contain the created test case id and version information. Failed entries will have
        /// Id = -1 and VersionNumber = 0 to indicate failure.
        /// </returns>
        /// <remarks>
        /// The controller delegates processing to the service layer which performs validation and persistence.
        /// The returned collection preserves one response per input request.
        /// </remarks>
        [HttpPost("bulk")]
        [ProducesResponseType(typeof(ICollection<CreateTestCaseResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ICollection<CreateTestCaseResponse>>> CreateBulkAsync([FromBody] ICollection<CreateTestCaseRequest> requests, CancellationToken ct)
        {
            _logger.LogDebug("TestCaseController.CreateBulk() start!");

            var responses = await _testCaseService.CreateAsync(requests, ct);
            return Ok(responses);
        }

        /// <summary>
        /// Attempts to create multiple test cases from the provided requests only if they do not already exist, and returns per-request responses.
        /// </summary>
        /// <param name="requests">Collection of <see cref="CreateTestCaseRequest"/> objects to process.</param>
        /// <returns>
        /// A collection of <see cref="CreateTestCaseResponse"/> objects describing the result for each request.
        /// Successful entries contain the created test case id and version information. Failed entries will have
        /// Id = -1 and VersionNumber = 0 to indicate failure.
        /// </returns>
        /// <remarks>
        /// The controller delegates processing to the service layer which performs validation and persistence.
        /// The returned collection preserves one response per input request.
        /// </remarks>
        [HttpPost("bulk/createIfNotExists")]
        [ProducesResponseType(typeof(ICollection<CreateTestCaseResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ICollection<CreateTestCaseResponse>>> CreateIfNotExistsAsync([FromBody] ICollection<CreateTestCaseRequest> requests, CancellationToken ct)
        {
            _logger.LogDebug("TestCaseController.CreateIfNotExistsAsync() start!");

            var responses = await _testCaseService.CreateIfNotExistsAsync(requests, ct);
            return Ok(responses);
        }

        /// <summary>
        /// Updates a test case by creating a new version. The request may include optional name and description
        /// values; if omitted the latest version's values are reused. Returns the created version details.
        /// </summary>
        /// <param name="request">Request payload identifying the test case to update and optional fields to change.</param>
        /// <param name="ct">Cancellation token to cancel the request.</param>
        /// <returns>
        /// The <see cref="UpdateTestCaseResponse"/> containing the new version information for the updated test case.
        /// </returns>
        [HttpPut("id")]
        [ProducesResponseType(typeof(UpdateTestCaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UpdateTestCaseResponse>> UpdateAsync(long id, [FromBody] UpdateTestCaseRequest request, CancellationToken ct = default)
        {
            _logger.LogDebug("TestCaseController.CreateBulk() start!");

            var response = await _testCaseService.UpdateAsync(request, ct);
            return Ok(response);
        }
    }
}
