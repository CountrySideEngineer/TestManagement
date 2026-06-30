using Microsoft.AspNetCore.Mvc;
using TestManagement.API.Features.TestResult.Create;
using TestManagement.API.Features.TestResult.Get;
using TestManagement.API.Models;
using TestManagement.API.Models.Report.Xml;
using TestManagement.API.Models.Requests;
using TestManagement.API.Services;

namespace TestManagement.API.Controllers
{
    /// <summary>
    /// API controller for managing test results.
    /// Provides endpoints for retrieving, creating, and importing test results.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TestResultController : Controller
    {
        /// <summary>
        /// Service for handling test result business logic and data operations.
        /// </summary>
        private readonly TestResultService _testResultService;

        /// <summary>
        /// Logger instance for recording controller events and debugging information.
        /// </summary>
        private readonly ILogger<TestResultController> _logger;

        /// <summary>
        /// Initializes a new instance of the TestResultController class.
        /// </summary>
        /// <param name="logger">The logger instance for logging controller operations.</param>
        /// <param name="testResultService">The test result service for data operations.</param>
        public TestResultController(ILogger<TestResultController> logger, TestResultService testResultService)
        {
            _logger = logger;
            _testResultService = testResultService;
        }

        /// <summary>
        /// Retrieves all test results asynchronously.
        /// </summary>
        /// <returns>An IActionResult containing a list of all test results with HTTP 200 OK status.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ICollection<GetTestResultResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ICollection<GetTestResultResponse>>> GetAllTestResultsAsync()
        {
            _logger.LogDebug("TestResultController.GetAllTestResults() start!");

            var testResults = await _testResultService.GetAllAsync();
            return Ok(testResults);
        }

        /// <summary>
        /// Retrieves a specific test result by its identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the test result to retrieve.</param>
        /// <returns>An IActionResult containing the test result with HTTP 200 OK status.</returns>
        [HttpGet("{id}", Name = "GetByIdAsync")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GetTestResultResponse>> GetByIdAsync(int id)
        {
            _logger.LogDebug("TestResultController.GetById() start!");

            TestResult testResult = await _testResultService.GetByIdAsync(id);
            return Ok(testResult);
        }

        /// <summary>
        /// Creates a single test result from the provided request asynchronously.
        /// Maps the request DTO to the domain model before persisting.
        /// </summary>
        /// <param name="request">The test result creation request containing test result details.</param>
        /// <returns>An IActionResult with HTTP 201 Created status and the created test result.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(CreateTestResultResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CreateTestResultResponse>> CreateAsync([FromBody] TestResultCreateRequest request)
        {
            _logger.LogDebug("TestResultController.Create() start!");

            var testResultRequest = new CreateTestResultRequest()
            {
                TestExecutionItemId = request.TestExecutionItemId,
                TestCaseId = request.TestCaseId,
                TestCaseVersionNumber = request.TestCaseVersionNumber,
                TestLevelId = request.TestLevelId,
                Message = request.Message,
                ExecutedAt = request.ExecutedAt,
                TestResultStatus = request.TestResultStatus
            };
            CreateTestResultResponse response = await _testResultService.CreateAsync(testResultRequest);

            return CreatedAtAction(
                nameof(CreateAsync),
                new { id = response.ResultId },
                response);
        }

        /// <summary>
        /// Creates multiple test results from a collection of requests asynchronously.
        /// Maps each request DTO to the domain model before persisting.
        /// </summary>
        /// <param name="requests">A collection of test result creation requests.</param>
        /// <returns>An IActionResult with HTTP 201 Created status and the created test results.</returns>
        [HttpPost("Bulk")]
        [ProducesResponseType(typeof(ICollection<TestResultCreateResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ICollection<TestResultCreateResponse>> CreateBulkAsync([FromBody] IEnumerable<TestResultCreateRequest> requests)
        {
            _logger.LogDebug("TestResultController.CreateBulk() start!");

            var testResults = new List<CreateTestResultRequest>();
            foreach (var request in requests)
            {
                var testResultRequest = new CreateTestResultRequest()
                {
                    TestExecutionItemId = request.TestExecutionItemId,
                    TestCaseId = request.TestCaseId,
                    TestCaseVersionNumber = request.TestCaseVersionNumber,
                    TestLevelId = request.TestLevelId,
                    Message = request.Message,
                    ExecutedAt = request.ExecutedAt,
                    TestResultStatus = request.TestResultStatus
                };
                testResults.Add(testResultRequest);
            }

            var testResultResponses = await _testResultService.CreateAsync(testResults);

            var responses = new List<TestResultCreateResponse>();
            foreach (var testResultResponse in testResultResponses)
            {
                var response = new TestResultCreateResponse()
                {
                    Id = testResultResponse.ResultId,
                    TestExecutionItemId = testResultResponse.TestExecutionItemId,
                    TestCaseId = testResultResponse.TestCaseId,
                    TestCaseVersionNumber = testResultResponse.TestCaseVersionNumber,
                    TestLevelId = testResultResponse.TestLevelId,
                    Message = testResultResponse.Message,
                    ExecutedAt = testResultResponse.ExecutedAt,
                    TestResultStatus = testResultResponse.TestResultStatus
                };
                responses.Add(response);
            }

            return responses;
        }
    }
}
