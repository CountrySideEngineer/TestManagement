using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using TestManagement.API.Features.Environment.Create;
using TestManagement.API.Features.Environment.Get;
using TestManagement.API.Services;

namespace TestManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnvironmentController : ControllerBase
    {
        private readonly EnvironmentService _environmentService;

        private readonly ILogger<EnvironmentController> _logger;

        public EnvironmentController(ILogger<EnvironmentController> logger, EnvironmentService environmentService)
        {
            _logger = logger;
            _environmentService = environmentService;
        }

        [HttpGet]
        public async Task<ICollection<GetEnvironmentResponse>> GetAllAsync()
        {
            _logger.LogDebug("EnvironmentController.GetAllAsync start!");

            return await _environmentService.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<GetEnvironmentResponse> GetById(int id)
        {
            _logger.LogDebug("EnvironmentController.GetAllAsync start!");

            return await _environmentService.GetByIdAsync(id);
        }

        [HttpPost]
        public async Task<CreateEnvironmentResponse> CreateAsync([FromBody] CreateEnvironmentRequest request, CancellationToken ct = default)
        {
            _logger.LogDebug("EnvironmentController.CreateAsync start!");

            return await _environmentService.CreateAsync(request, ct);
        }
    }
}
