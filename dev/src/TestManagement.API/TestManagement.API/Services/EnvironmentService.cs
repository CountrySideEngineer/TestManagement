using Microsoft.EntityFrameworkCore;
using TestManagement.API.Data;
using TestManagement.API.Features.Environment.Create;
using TestManagement.API.Features.Environment.Get;

namespace TestManagement.API.Services
{
    /// <summary>
    /// Service responsible for environment-related use cases.
    /// Provides retrieval operations for environments and their historical versions.
    /// </summary>
    public class EnvironmentService
    {
        /// <summary>
        /// Database context used to access environment and version data.
        /// </summary>
        private readonly TestManagementDbContext _dbContext;

        /// <summary>
        /// Logger used for diagnostic and trace messages. May be null in some test scenarios.
        /// </summary>
        private readonly ILogger<EnvironmentService>? _logger = null;

        /// <summary>
        /// Constructs a new instance of <see cref="EnvironmentService"/>.
        /// </summary>
        /// <param name="dbContext">The <see cref="TestManagementDbContext"/> used for data access.</param>
        /// <param name="logger">Logger instance for diagnostics.</param>
        public EnvironmentService(
            TestManagementDbContext dbContext,
            ILogger<EnvironmentService> logger
            )
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all environments with their versions and projects them to response DTOs.
        /// </summary>
        /// <returns>
        /// A collection of <see cref="GetEnvironmentResponse"/> containing environment name and version details.
        /// </returns>
        public async Task<ICollection<GetEnvironmentResponse>> GetAllAsync()
        {
            _logger?.LogDebug("EnvironmentService::GetAllAsync() start!");

            var envionments = await _dbContext.Environments
                .Include(_ => _.Versions)
                .ToListAsync();
            var responses = new List<GetEnvironmentResponse>();
            foreach (var environment in envionments)
            {
                foreach (var version in environment.Versions)
                {
                    var response = new GetEnvironmentResponse
                    {
                        Name = environment.Name,
                        Os = version.Os,
                        RunTime = version.RunTime
                    };
                    responses.Add(response);
                }
            }

            return responses;
        }

        /// <summary>
        /// Retrieves a single environment by id and returns the latest version information.
        /// </summary>
        /// <param name="id">Identifier of the environment to retrieve.</param>
        /// <returns>
        /// A <see cref="GetEnvironmentResponse"/> containing the environment name and the latest version's OS and runtime.
        /// If the environment is not found an empty response DTO is returned.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown when an error occurs while querying the database.</exception>
        public async Task<GetEnvironmentResponse> GetByIdAsync(int id)
        {
            _logger?.LogDebug("EnvironmentService::GetById() start!");
            _logger?.LogDebug("id = {Id}", id);

            try
            {
                var environment = await _dbContext.Environments
                    .Where(_ => _.Id == id)
                    .Include(_ => _.Versions)
                    .FirstOrDefaultAsync();
                var response = new GetEnvironmentResponse();
                if (null != environment)
                {
                    var version = environment.Versions.OrderByDescending(_ => _.VersionNumber).FirstOrDefault();
                    response.Name = environment.Name;
                    response.Os = version!.Os;
                    response.RunTime = version.RunTime;
                }

                return response;
            }
            catch (Exception)
            {
                throw new ArgumentException();
            }
        }

        public async Task<CreateEnvironmentResponse> CreateAsync(CreateEnvironmentRequest request, CancellationToken ct = default)
        {
            _logger?.LogDebug("EnvironmentService::CreateAsync() start!");
            _logger?.LogDebug("request = {@Request}", request);

            var isExists = await _dbContext.Environments.AnyAsync(_ => _.Name == request.Name, ct);
            if (isExists)
            {
                throw new ArgumentException($"Environment with name '{request.Name}' already exists.");
            }

            var newEnvironment = new Models.Environment()
            {
                Name = request.Name
            };
            newEnvironment.AddVersion(request.Os, request.RunTime);
            _dbContext.Environments.Add(newEnvironment);

            try
            {
                await _dbContext.SaveChangesAsync(ct);

            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error occurred while creating environment.");
                throw;
            }

            var createdEnvironment = newEnvironment.Versions
                .OrderByDescending(_ => _.VersionNumber)
                .FirstOrDefault();
            if (null == createdEnvironment)
            {
                throw new InvalidOperationException("Failed to create environment version.");
            }
            var response = new CreateEnvironmentResponse
            {
                Id = createdEnvironment.Id,
                Name = newEnvironment.Name,
                Os = createdEnvironment.Os,
                RunTime = createdEnvironment.RunTime,
                VersionNumber = createdEnvironment.VersionNumber
            };
            return response;
        }
    }
}
