using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading;
using TestManagement.API.Features.Project.Get;
using TestManagement.API.Models;
using TestManagement.API.Services;

namespace TestManagement.API.Controllers
{
    /// <summary>
    /// API controller that handles operations related to projects.
    /// Uses a facade service to keep the controller thin.
    /// </summary>
    public class ProjectController : Controller
    {
        /// <summary>
        /// Facade service that coordinates project and composition operations.
        /// </summary>
        private readonly IProjectFacadeService _projectFacadeService;

        /// <summary>
        /// Logger instance for controller diagnostics.
        /// </summary>
        private readonly ILogger<ProjectController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectController"/> class.
        /// </summary>
        /// <param name="projectFacadeService">Facade service used to perform project-related use cases.</param>
        /// <param name="logger">Logger for controller diagnostics.</param>
        public ProjectController(
            IProjectFacadeService projectFacadeService,
            ILogger<ProjectController> logger
            )
        {
            _projectFacadeService = projectFacadeService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all projects.
        /// </summary>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>ActionResult containing a collection of <see cref="GetProjectResponse"/>.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ICollection<GetProjectResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ICollection<GetProjectResponse>>> GetAllAsync(CancellationToken ct = default)
        {
            _logger.LogDebug("ProjectController::GetAllAsync() start!");
            var projects = await _projectFacadeService.GetAllAsync(ct);
            return Ok(projects);
        }

        /// <summary>
        /// Retrieves a single project by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the project to retrieve.</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>ActionResult containing the <see cref="GetProjectResponse"/> for the specified id, or NotFound if missing.</returns>
        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(GetProjectResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GetProjectResponse>> GetByIdAsync(long id, CancellationToken ct = default)
        {
            _logger.LogDebug("ProjectController::GetByIdAsync() start!");
            var project = await _projectFacadeService.GetByIdAsync(id, ct);
            if (project == null) return NotFound();
            return Ok(project);
        }

        /// <summary>
        /// Retrieves compositions associated with a specific project.
        /// </summary>
        /// <param name="id">Project identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>ActionResult containing a collection of <see cref="ProjectComposition"/>.</returns>
        [HttpGet("{id:long}/compositions")]
        [ProducesResponseType(typeof(ICollection<ProjectComposition>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ICollection<ProjectComposition>>> GetCompositionsByProjectIdAsync(long id, CancellationToken ct = default)
        {
            _logger.LogDebug("ProjectController::GetCompositionsByProjectIdAsync() start! ProjectId: {Id}", id);
            var comps = await _projectFacadeService.GetCompositionsByProjectIdAsync(id, ct);    
            return Ok(comps);
        }

        /// <summary>
        /// Creates a composition linking the specified project and test suite.
        /// </summary>
        /// <param name="projectId">Project identifier.</param>
        /// <param name="testSuiteId">Test suite identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Created composition and a 201 Created response.</returns>
        [HttpPost("{projectId:long}/compositions/{testSuiteId:long}")]
        [ProducesResponseType(typeof(ProjectComposition), StatusCodes.Status201Created)]
        public async Task<ActionResult<ProjectComposition>> CreateCompositionAsync(long projectId, long testSuiteId, CancellationToken ct = default)
        {
            _logger.LogDebug("ProjectController::CreateCompositionAsync() start! ProjectId: {ProjectId}, TestSuiteId: {TestSuiteId}", projectId, testSuiteId);
            var created = await _projectFacadeService.CreateCompositionAsync(projectId, testSuiteId, ct);
            return CreatedAtAction(nameof(GetCompositionsByProjectIdAsync), new { id = projectId }, created);
        }

        /// <summary>
        /// Deletes a composition by its identifier.
        /// </summary>
        /// <param name="id">Composition identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>NoContent on success, NotFound if the composition does not exist.</returns>
        [HttpDelete("compositions/{id:long}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCompositionAsync(long id, CancellationToken ct = default)
        {
            _logger.LogDebug("ProjectController::DeleteCompositionAsync() start! Id: {Id}", id);
            var deleted = await _projectFacadeService.DeleteCompositionAsync(id, ct);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
