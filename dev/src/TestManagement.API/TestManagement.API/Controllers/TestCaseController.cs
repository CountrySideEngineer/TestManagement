using Microsoft.AspNetCore.Mvc;
using TestManagement.API.Features.TestCases.Create;
using TestManagement.API.Models;
using TestManagement.API.Services;

namespace TestManagement.API.Controllers
{
    /// <summary>
    /// API controller that exposes endpoints for managing test cases and their versions.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TestCaseController : ControllerBase
    {
        private readonly TestCaseService _testCaseService;
        private readonly ILogger<TestCaseController> _logger;

        /// <summary>
        /// Creates a new instance of <see cref="TestCaseController"/>.
        /// </summary>
        /// <param name="logger">Logger instance used for diagnostic messages.</param>
        /// <param name="testCaseService">Service that encapsulates test case use-cases.</param>
        public TestCaseController(ILogger<TestCaseController> logger, TestCaseService testCaseService)
        {
            _logger = logger;
            _testCaseService = testCaseService;
        }

        /// <summary>
        /// Returns all test case versions including their associated test level information.
        /// </summary>
        /// <returns>HTTP 200 with the collection of test case versions.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllTestCasesAsync()
        {
            _logger.LogDebug("TestCaseController.GetAllTestCases() start!");

            var testCases = await _testCaseService.GetAllAsync();
            return Ok(testCases);
        }

        /// <summary>
        /// Returns all test case versions that belong to the specified test level.
        /// </summary>
        /// <param name="id">Identifier of the test level to filter test cases by.</param>
        /// <returns>HTTP 200 with the collection of matching test case versions.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            _logger.LogDebug("TestCaseController.GetById() start!");

            var testCases = await _testCaseService.GetByTestLevelIdAsync(id);
            return Ok(testCases);
        }

        /// <summary>
        /// Creates a new test case with an initial version.
        /// </summary>
        /// <param name="request">Request payload containing code, name, description and test level id.</param>
        /// <returns>Response containing the created test case version details.</returns>
        [HttpPost]
        public async Task<CreateTestCaseResponse> CreateAsync([FromBody] CreateTestCaseRequest request)
        {
            _logger.LogDebug("TestCaseController.Create() start!");
            var response = await _testCaseService.CreateAsync(request);
            return response;
        }

        /// <summary>
        /// Bulk creates test case versions.
        /// </summary>
        /// <param name="testCases">Collection of <see cref="TestCaseVersion"/> entities to create.</param>
        /// <returns>HTTP 201 Created with a reference to the created resources.</returns>
        [HttpPost("Bulk")]
        public async Task<IActionResult> CreateBulkAsync(List<TestCaseVersion> testCases)
        {
            _logger.LogDebug("TestCaseController.CreateBulk() start!");

            await _testCaseService.CreateAsync(testCases);

            return CreatedAtAction(nameof(GetByIdAsync), new { id = testCases[0].Id }, testCases);
        }
    }
}
