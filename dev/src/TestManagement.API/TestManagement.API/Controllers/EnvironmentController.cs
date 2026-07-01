using Microsoft.AspNetCore.Mvc;
using Npgsql.Replication;
using System.Runtime.CompilerServices;
using TestManagement.API.Features.Environment.Create;
using TestManagement.API.Features.Environment.Get;
using TestManagement.API.Features.Environment.Update;
using TestManagement.API.Models.Requests;
using TestManagement.API.Services;

namespace TestManagement.API.Controllers
{
    /// <summary>
    /// API controller that exposes endpoints for managing environments and their versions.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class EnvironmentController : Controller
    {
        /// <summary>
        /// Service that encapsulates environment-related use cases and data access.
        /// </summary>
        private readonly IEnvironmentService _environmentService;

        /// <summary>
        /// Logger used for diagnostic messages within the controller.
        /// </summary>
        private readonly ILogger<EnvironmentController> _logger;

        /// <summary>
        /// Creates a new instance of <see cref="EnvironmentController"/>.
        /// </summary>
        /// <param name="logger">Logger instance used for diagnostic messages.</param>
        /// <param name="environmentService">Service that provides environment use-cases.</param>
        public EnvironmentController(
            ILogger<EnvironmentController> logger,
            IEnvironmentService environmentService
            )
        {
            _logger = logger;
            _environmentService = environmentService;
        }

        /// <summary>
        /// Returns all environments along with their versions.
        /// </summary>
        /// <returns>Collection of <see cref="GetEnvironmentResponse"/> DTOs.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ICollection<GetEnvironmentResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ICollection<GetEnvironmentResponse>>> GetAllAsync(CancellationToken ct = default)
        {
            _logger.LogDebug("EnvironmentController.GetAllAsync start!");

            var result = await _environmentService.GetAllAsync(ct);
            return Ok(result);
        }

        /// <summary>
        /// Returns all versions for the environment identified by the given id.
        /// </summary>
        /// <param name="id">Identifier of the environment whose versions will be retrieved.</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>A collection of <see cref="GetEnvironmentResponse"/> DTOs representing versions associated with the environment.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ICollection<GetEnvironmentResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ICollection<GetEnvironmentResponse>>> GetByIdAsync(int id, CancellationToken ct = default)
        {
            _logger.LogDebug("EnvironmentController.GetByIdAsync start!");

            ICollection<GetEnvironmentResponse> responses = await _environmentService.GetByIdAsync(id, ct);
            return Ok(responses);
        }

        /// <summary>
        /// Retrieves all environment versions that match the specified environment name.
        /// Returns a collection of version DTOs for environments with the given name.
        /// </summary>
        /// <param name="name">The environment name to query for (case-sensitive depending on the data store).</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>A collection of <see cref="GetEnvironmentResponse"/> instances representing matching environment versions.</returns>
        [HttpGet("name/{name}")]
        [ProducesResponseType(typeof(ICollection<GetEnvironmentResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ICollection<GetEnvironmentResponse>>> GetByNameAsync(string name, CancellationToken ct = default)
        {
            _logger.LogDebug("EnvironmentController.GetByNameAsync start!");

            ICollection<GetEnvironmentResponse> responses = await _environmentService.GetByNameAsync(name, ct);
            return Ok(responses);
        }

        /// <summary>
        /// Creates a new environment along with its initial version.
        /// </summary>
        /// <param name="request">Request DTO containing name, OS and runtime information for the new environment.</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>A <see cref="CreateEnvironmentResponse"/> describing the created environment version.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(CreateEnvironmentResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CreateEnvironmentResponse>> CreateAsync([FromBody] CreateEnvironmentRequest request, CancellationToken ct = default)
        {
            _logger.LogDebug("EnvironmentController.CreateAsync start!");

            CreateEnvironmentResponse response = await _environmentService.CreateAsync(request, ct);

            // Return 201 Created. Location references the GetByIdAsync route for the parent environment.
            return CreatedAtAction(
                nameof(GetByIdAsync),
                new { id = response.Id },
                response);
        }

        /// <summary>
        /// Updates an existing environment by adding a new version.
        /// </summary>
        /// <param name="request">Request DTO containing the environment identifier and new version details.</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>An <see cref="UpdateEnvironmentResponse"/> describing the created version.</returns>
        [HttpPut]
        [ProducesResponseType(typeof(UpdateEnvironmentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UpdateEnvironmentResponse>> UpdateAsync([FromBody] UpdateEnvironmentRequest request, CancellationToken ct = default)
        {
            _logger.LogDebug("EnvironmentController.UpdateAsync start!");

            UpdateEnvironmentResponse response = await _environmentService.UpdateAsync(request, ct);
            return Ok(response);
        }
    }
}
