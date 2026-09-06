using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using TestManagement.API.Features.Project.Get;
using TestManagement.API.Services;

namespace TestManagement.API.Controllers
{
    /// <summary>
    /// API controller that handles operations related to projects.
    /// Provides endpoints to retrieve all projects and a single project by id.
    /// </summary>
    public class ProjectController : Controller
    {
        /// <summary>
        /// Service that contains project-related business logic and data access operations.
        /// </summary>
        private readonly IProjectService _projectService;

        /// <summary>
        /// Logger instance used to write diagnostic messages from this controller.
        /// </summary>
        private readonly ILogger<ProjectController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectController"/> class.
        /// </summary>
        /// <param name="projectService">Service used to perform project operations.</param>
        /// <param name="logger">Logger for controller diagnostics.</param>
        public ProjectController(
            IProjectService projectService,
            ILogger<ProjectController> logger
            )
        {
            _projectService = projectService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ICollection<GetProjectResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        /// <summary>
        /// Retrieves all projects.
        /// </summary>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>ActionResult containing a collection of <see cref="GetProjectResponse"/>.</returns>
        public async Task<ActionResult<ICollection<GetProjectResponse>>> GetAllAsync(CancellationToken ct = default)
        {
            _logger.LogDebug("ProjectController::GetAllAsync() start!");

            var projects = await _projectService.GetAllAsync(ct);
            return Ok(projects);
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetProjectResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        /// <summary>
        /// Retrieves a single project by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the project to retrieve.</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>ActionResult containing the <see cref="GetProjectResponse"/> for the specified id.</returns>
        public async Task<ActionResult<GetProjectResponse>> GetByIdAsync(long id, CancellationToken ct = default)
        {
            _logger.LogDebug("ProjectController::GetByIdAsync() start!");

            var projects = await _projectService.GetByIdAsync(id, ct);

            return Ok(projects);
        }
    }
}
