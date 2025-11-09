using Microsoft.AspNetCore.Mvc;
using TestManagement.Model;
using TestManagement.Serivces;

namespace TestManagement.Controllers
{
    [Route("api/[conttroller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly ProjectService _projectService;

        public ProjectController(ProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProjects()
        {
            var projects = await _projectService.GetAllAsync();
            return Ok(projects);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var project = await _projectService.GetProjectWithSuiteAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            return Ok(project);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Project project)
        {
            await _projectService.AddAsync(project);
            return CreatedAtAction(nameof(Get), new { id = project.Id }, project);
        }
    }
}
