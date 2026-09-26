using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading;
using TestManagement.API.Features.Project.Create;
using TestManagement.API.Features.Project.Get;
using TestManagement.API.Features.TestCases.Create;
using TestManagement.API.Models;
using TestManagement.API.Services;

namespace TestManagement.API.Controllers;

/// <summary>
/// API controller that handles operations related to projects.
/// Uses a facade service to keep the controller thin.
/// </summary>
[ApiController]
[Route("api/projects")]
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
    /// <returns>ActionResult containing a collection of <see cref="Features.Project.Get.GetProjectResponse"/>.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ICollection<Features.Project.Get.GetProjectResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ICollection<Features.Project.Get.GetProjectResponse>>> GetAllAsync(CancellationToken ct = default)
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
    /// <returns>ActionResult containing the <see cref="Features.Project.Get.GetProjectResponse"/> for the specified id, or NotFound if missing.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Features.Project.Get.GetProjectResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ActionName(nameof(GetByIdAsync))]
    public async Task<ActionResult<Features.Project.Get.GetProjectResponse>> GetByIdAsync(long id, CancellationToken ct = default)
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
    /// <returns>ActionResult containing a collection of <see cref="Models.ProjectTestSuiteComposition"/>.</returns>
    [HttpGet("{id:long}/compositions/testsuite")]
    [ProducesResponseType(typeof(ICollection<Models.ProjectTestSuiteComposition>), StatusCodes.Status200OK)]
    [ActionName(nameof(GetByIdWithTestSuitesAsync))]
    public async Task<ActionResult<ICollection<Models.ProjectTestSuiteComposition>>> GetByIdWithTestSuitesAsync(
        long id, 
        CancellationToken ct = default)
    {
        _logger.LogDebug("ProjectController::GetCompositionsByProjectIdAsync() start! ProjectId: {Id}", id);

        var comps = await _projectFacadeService.GetByIdWithTestSuitesAsync(id, ct);    
        return Ok(comps);
    }

    /// <summary>
    /// Creates a new project from the specified <see cref="CreateProjectRequest"/>.
    /// </summary>
    /// <remarks>
    /// The endpoint delegates creation to the project facade service and returns a
    /// 201 Created response with a Location header that points to <see cref="GetByIdAsync"/>.
    /// </remarks>
    /// <param name="request">Request DTO containing the data required to create the project.</param>
    /// <param name="ct">Cancellation token to cancel the operation.</param>
    /// <returns>
    /// An <see cref="ActionResult{CreateProjectResponse}"/> containing the created project
    /// and an HTTP 201 Created status on success.
    /// Possible responses:
    ///  - 201 Created: Project successfully created.
    ///  - 400 Bad Request: Request validation failed.
    ///  - 500 Internal Server Error: Unexpected server error.
    /// </returns>
    [HttpPost]
    [ProducesResponseType(typeof(CreateProjectResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CreateProjectResponse>> CreateProjectAsync(
        CreateProjectRequest request,
        CancellationToken ct = default)
    {
        _logger.LogDebug("ProjectController::CreateProjectAsync() start! Request: {@Request}", request);

        var created = await _projectFacadeService.CreateProjectAsync(request, ct);

        var actionResult = CreatedAtAction(
            nameof(GetByIdAsync),
            new { id = created.Id },
            created);

        return actionResult;
    }

    /// <summary>
    /// Creates a composition linking the specified project and test suite.
    /// </summary>
    /// <param name="projectId">Project identifier.</param>
    /// <param name="testSuiteId">Test suite identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Created composition and a 201 Created response.</returns>
    [HttpPost("{projectId:long}/compositions/testsuite")]
    [ProducesResponseType(typeof(Models.ProjectTestSuiteComposition), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Models.ProjectTestSuiteComposition>> CreateCompositionAsync(
        long projectId, 
        ProjectTestSuiteCompositionCreateRequest request,
        CancellationToken ct = default)
    {
        _logger.LogDebug("ProjectController::CreateCompositionAsync() start! ProjectId: {ProjectId}, TestSuiteId: {TestSuiteId}",
            projectId, 
            request.TestSuiteId);

        var createRequest = new CreateProjectTestSuiteCompositionRequest
        {
            ProjetId = projectId,
            TestSuiteId = request.TestSuiteId
        };


        var created = await _projectFacadeService.CreateTestSuiteCompositionAsync(createRequest, ct);
        return CreatedAtAction(nameof(GetByIdWithTestSuitesAsync), new { id = projectId }, created);
    }

    [HttpPost("{projectId:long}/compositions/testsuites")]
    [ProducesResponseType(typeof(Models.ProjectTestSuiteComposition), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ICollection<ProjectTestSuiteCompositionCreateResponse>>> CreateCompositionAsync(
        long projectId,
        ICollection<ProjectTestSuiteCompositionCreateRequest> requests,
        CancellationToken ct = default)
    {
        _logger.LogDebug("ProjectController::CreateCompositionAsync() start! ProjectId: {ProjectId}",
            projectId);

        var createRequests = requests.Select(request => new CreateProjectTestSuiteCompositionRequest
            {
                ProjetId = projectId,
                TestSuiteId = request.TestSuiteId
            })
            .ToList();

        var created = await _projectFacadeService.CreateTestSuiteCompositionAsync(createRequests, ct);
        var createdResponse = created.Select(_ => new ProjectTestSuiteCompositionCreateResponse
            {
                CompositionId = _.CompositionId,
                TestSuiteId = _.TestSuiteId
            })
            .ToList();
        return Ok(createdResponse);
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
