using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using TestManagement.API.Data;
using TestManagement.API.Features.Environment.Create;
using TestManagement.API.Features.Environment.Get;
using TestManagement.API.Features.Environment.Update;

namespace TestManagement.API.Services
{
    /// <summary>
    /// Service responsible for environment-related use cases.
    /// Provides retrieval operations for environments and their historical versions.
    /// </summary>
    public class EnvironmentService : IEnvironmentService
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
        public async Task<ICollection<GetEnvironmentResponse>> GetAllAsync(CancellationToken ct = default)
        {
            _logger?.LogDebug("EnvironmentService::GetAllAsync() start!");

            var environments = await _dbContext.Environments
                .Include(_ => _.Versions)
                .ToListAsync(ct);

            ICollection<GetEnvironmentResponse> responses = environments
                .Select(env => new GetEnvironmentResponse
                {
                    Id = env.Id,
                    Name = env.Name,
                    Versions = env.Versions.Select(version => new Features.Environment.EnvironmentVersion
                    {
                        VersionNumber = version.VersionNumber,
                        Os = version.Os,
                        RunTime = version.RunTime,
                        IsLatest = version.IsLatest
                    })
                    .ToList()
                })
                .ToList();
            return responses;
        }

        /// <summary>
        /// Retrieves all versions for the environment identified by the given id and projects
        /// each version to a <see cref="GetEnvironmentResponse"/> DTO.
        /// </summary>
        /// <param name="id">Identifier of the environment to retrieve.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// A collection of <see cref="GetEnvironmentResponse"/> where each item represents
        /// a version of the requested environment. If the environment or its versions are
        /// not found an empty collection is returned.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown when an error occurs while querying the database.</exception>
        public async Task<ICollection<GetEnvironmentResponse>> GetByIdAsync(int id, CancellationToken ct = default)
        {
            _logger?.LogDebug("EnvironmentService::GetByIdAsync() start!");
            _logger?.LogDebug("id = {Id}", id);

            try
            {
                var environments = await _dbContext.Environments
                    .Where(e => e.Id == id)
                    .Include(e => e.Versions)
                    .ToListAsync(ct);
                
                ICollection<GetEnvironmentResponse> responses = environments
                    .Select(env => new GetEnvironmentResponse
                    {
                        Id = env.Id,
                        Name = env.Name,
                        Versions = env.Versions.Select(version => new Features.Environment.EnvironmentVersion
                        {
                            VersionNumber = version.VersionNumber,
                            Os = version.Os,
                            RunTime = version.RunTime,
                            IsLatest = version.IsLatest
                        })
                        .ToList()
                    })
                    .ToList();

                return responses;
            }
            catch (Exception)
            {
                throw new ArgumentException();
            }
        }

        /// <summary>
        /// Retrieves all versions for environments that match the provided name.
        /// </summary>
        /// <param name="name">The environment name to search for.</param>
        /// <param name="ct">Cancellation token to cancel the database query.</param>
        /// <returns>
        /// A collection of <see cref="GetEnvironmentResponse"/> DTOs representing each matching environment version.
        /// If no environments match the given name an empty collection is returned.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown when an error occurs while querying the database.</exception>
        public async Task<ICollection<GetEnvironmentResponse>> GetByNameAsync(string name, CancellationToken ct = default)
        {
            _logger?.LogDebug("EnvironmentService::GetByNameAsync() start!");
            _logger?.LogDebug("name = {Name}", name);
            try
            {
                var environments = await _dbContext.Environments
                    .Where(e => e.Name == name)
                    .Include(e => e.Versions)
                    .ToListAsync();

                ICollection<GetEnvironmentResponse> responses = environments
                    .Select(env => new GetEnvironmentResponse
                    {
                        Id = env.Id,
                        Name = env.Name,
                        Versions = env.Versions.Select(version => new Features.Environment.EnvironmentVersion
                        {
                            VersionNumber = version.VersionNumber,
                            Os = version.Os,
                            RunTime = version.RunTime,
                            IsLatest = version.IsLatest
                        })
                        .ToList()
                    })
                    .ToList();

                return responses;
            }
            catch (Exception)
            {
                throw new ArgumentException();
            }
        }

        /// <summary>
        /// Creates a new environment with an initial version and persists it to the database.
        /// </summary>
        /// <param name="request">Request containing the environment name, OS and runtime information.</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>
        /// A <see cref="CreateEnvironmentResponse"/> describing the created environment version.
        /// </returns>
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

            var response = new CreateEnvironmentResponse
            {
                Id = newEnvironment.Id,
                Name = newEnvironment.Name,
                Version = new Features.Environment.EnvironmentVersion
                {
                    Id = newEnvironment.Versions.ElementAt(0).EnvironmentId,
                    Os = newEnvironment.Versions.ElementAt(0).Os,
                    RunTime = newEnvironment.Versions.ElementAt(0).RunTime,
                    VersionNumber = newEnvironment.Versions.ElementAt(0).VersionNumber,
                    IsLatest = newEnvironment.Versions.ElementAt(0).IsLatest
                }
            };
            return response;
        }

        public async Task<UpdateEnvironmentResponse> UpdateAsync(UpdateEnvironmentRequest request, CancellationToken ct = default)
        {
            _logger?.LogDebug("EnvironmentService::UpdateAsync() start!");
            _logger?.LogDebug("request = {@Request}", request);

            var isExists = await _dbContext.Environments.AnyAsync(_ => _.Name == request.Name);
            if (!isExists)
            {
                throw new ArgumentException($"Environment with name '{request.Name}' does not exist.");
            }

            var environment = await _dbContext.Environments
                .Where(_ => _.Name == request.Name)
                .Include(_ => _.Versions)
                .FirstOrDefaultAsync(ct);
            if (null == environment)
            {
                throw new ArgumentException();
            }

            var latestEnvironment = environment.Versions
                .OrderByDescending(_ => _.VersionNumber)
                .FirstOrDefault();
            if (null == latestEnvironment)
            {
                throw new Exception("No version exists for the environment.");
            }

            if ((latestEnvironment.Os == request.Os) && (latestEnvironment.RunTime == request.RunTime))
            {
                // No changes detected, throw an exception or return a specific response indicating no update was made.
                throw new InvalidOperationException("No changes detected in the environment version.");
            }
            environment.AddVersion(request.Os, request.RunTime);
            await _dbContext.SaveChangesAsync(ct);

            var newVersion = environment.Versions.Max(_ => _.VersionNumber);
            var response = new UpdateEnvironmentResponse
            {
                Name = environment.Name,
                Os = request.Os,
                RunTime = request.RunTime,
                VersionNumber = newVersion
            };
            return response;
        }
    }
}
