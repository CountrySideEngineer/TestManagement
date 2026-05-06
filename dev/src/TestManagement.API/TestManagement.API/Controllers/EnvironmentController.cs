using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using TestManagement.API.Features.Environment.Create;
using TestManagement.API.Features.Environment.Get;
using TestManagement.API.Features.Environment.Update;
using TestManagement.API.Services;

namespace TestManagement.API.Controllers
{
    /// <summary>
    /// API controller that exposes endpoints for managing environments and their versions.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class EnvironmentController : ControllerBase
    {
        /// <summary>
        /// Service that encapsulates environment-related use cases and data access.
        /// </summary>
        private readonly EnvironmentService _environmentService;

        /// <summary>
        /// Logger used for diagnostic messages within the controller.
        /// </summary>
        private readonly ILogger<EnvironmentController> _logger;

        /// <summary>
        /// Creates a new instance of <see cref="EnvironmentController"/>.
        /// </summary>
        /// <param name="logger">Logger instance used for diagnostic messages.</param>
        /// <param name="environmentService">Service that provides environment use-cases.</param>
        public EnvironmentController(ILogger<EnvironmentController> logger, EnvironmentService environmentService)
        {
            _logger = logger;
            _environmentService = environmentService;
        }

        /// <summary>
        /// Returns all environments along with their versions.
        /// </summary>
        /// <returns>Collection of <see cref="GetEnvironmentResponse"/> DTOs.</returns>
        [HttpGet]
        public async Task<ICollection<GetEnvironmentResponse>> GetAllAsync()
        {
            _logger.LogDebug("EnvironmentController.GetAllAsync start!");

            return await _environmentService.GetAllAsync();
        }

        /// <summary>
        /// Returns the latest version information for the environment identified by the given id.
        /// </summary>
        /// <param name="id">Identifier of the environment to retrieve.</param>
        /// <returns>A <see cref="GetEnvironmentResponse"/> DTO with the latest version details.</returns>
        [HttpGet("id/{id}")]
        public async Task<ICollection<GetEnvironmentResponse>> GetById(int id)
        {
            _logger.LogDebug("EnvironmentController.GetByIdAsync start!");

            return await _environmentService.GetByIdAsync(id);
        }

        /// <summary>
        /// Retrieves all environment versions that match the specified environment name.
        /// Returns a collection of version DTOs for environments with the given name.
        /// </summary>
        /// <param name="name">The environment name to query for (case-sensitive depending on the data store).</param>
        /// <returns>A collection of <see cref="GetEnvironmentResponse"/> instances representing matching environment versions.</returns>
        [HttpGet("name/{name}")]
        public async Task<ICollection<GetEnvironmentResponse>> GetByName(string name)
        {
            _logger.LogDebug("EnvironmentController.GetByNameAsync start!");

            return await _environmentService.GetByNameAsync(name);
        }

        /// <summary>
        /// Creates a new environment along with its initial version.
        /// </summary>
        /// <param name="request">Request DTO containing name, OS and runtime information for the new environment.</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>A <see cref="CreateEnvironmentResponse"/> describing the created environment version.</returns>
        [HttpPost]
        public async Task<CreateEnvironmentResponse> CreateAsync([FromBody] CreateEnvironmentRequest request, CancellationToken ct = default)
        {
            _logger.LogDebug("EnvironmentController.CreateAsync start!");

            return await _environmentService.CreateAsync(request, ct);
        }

        /// <summary>
        /// Updates an existing environment by adding a new version.
        /// </summary>
        /// <param name="request">Request DTO containing the environment identifier and new version details.</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>An <see cref="UpdateEnvironmentResponse"/> describing the created version.</returns>
        [HttpPut]
        public async Task<UpdateEnvironmentResponse> UpdateAsync([FromBody] UpdateEnvironmentRequest request, CancellationToken ct = default)
        {
            _logger.LogDebug("EnvironmentController.UpdateAsync start!");

            return await _environmentService.UpdateAsync(request, ct);
        }
    }
}
