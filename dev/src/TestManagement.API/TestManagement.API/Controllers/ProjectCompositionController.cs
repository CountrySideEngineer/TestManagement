using Microsoft.AspNetCore.Mvc;
using TestManagement.API.Models;
using TestManagement.API.Services;

namespace TestManagement.API.Controllers
{
    /// <summary>
    /// API controller to manage ProjectComposition entities.
    /// </summary>
    [ApiController]
    [Route("api/projectcompositions")]
    public class ProjectCompositionController : Controller
    {
        private readonly IProjectCompositionService _service;
        private readonly ILogger<ProjectCompositionController>? _logger;

        public ProjectCompositionController(
            ILogger<ProjectCompositionController>? logger,
            IProjectCompositionService service)
        {
            _logger = logger;
            _service = service;
        }

        /// <summary>
        /// Returns all project compositions.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ICollection<ProjectComposition>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ICollection<ProjectComposition>>> GetAllAsync(CancellationToken ct)
        {
            _logger?.LogDebug("ProjectCompositionController.GetAllAsync() start");
            var items = await _service.GetAllAsync(ct);
            return Ok(items);
        }

        /// <summary>
        /// Returns compositions for a specific project.
        /// </summary>
        [HttpGet("project/{projectId}")]
        [ProducesResponseType(typeof(ICollection<ProjectComposition>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ICollection<ProjectComposition>>> GetByProjectIdAsync(long projectId, CancellationToken ct)
        {
            _logger?.LogDebug("ProjectCompositionController.GetByProjectIdAsync({ProjectId}) start", projectId);
            var items = await _service.GetByProjectIdAsync(projectId, ct);
            return Ok(items);
        }

        /// <summary>
        /// Gets a composition by id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProjectComposition), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProjectComposition>> GetByIdAsync(long id, CancellationToken ct)
        {
            _logger?.LogDebug("ProjectCompositionController.GetByIdAsync({Id}) start", id);
            var item = await _service.GetByIdAsync(id, ct);
            if (item == null) return NotFound();
            return Ok(item);
        }

        /// <summary>
        /// Creates a new project composition.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ProjectComposition), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProjectComposition>> CreateAsync([FromBody] ProjectComposition request, CancellationToken ct)
        {
            _logger?.LogDebug("ProjectCompositionController.CreateAsync() start");
            if (request == null) return BadRequest();

            var created = await _service.CreateAsync(request.ProjectId, request.TestSuiteId, ct);

            return CreatedAtAction(
                nameof(GetByIdAsync),
                new { id = created.Id },
                created);
        }

        /// <summary>
        /// Deletes a composition by id.
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync(long id, CancellationToken ct)
        {
            _logger?.LogDebug("ProjectCompositionController.DeleteAsync({Id}) start", id);
            var ok = await _service.DeleteAsync(id, ct);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}