using Microsoft.EntityFrameworkCore;
using TestManagement.API.Data;
using TestManagement.API.Features.Environment.Get;

namespace TestManagement.API.Services
{
    public class EnvironmentService
    {
        private readonly TestManagementDbContext _dbContext;

        private readonly ILogger<EnvironmentService>? _logger = null;

        public EnvironmentService(
            TestManagementDbContext dbContext,
            ILogger<EnvironmentService> logger
            )
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<ICollection<GetEnvironmentResponse>> GetAllAsync()
        {
            _logger?.LogDebug("EnvironmentService::GetAllAsync() start!");

            var envionments = await _dbContext.Environments
                .Include(_ => _.Versions)
                .ToListAsync();
            var responses = new List<GetEnvironmentResponse>();
            foreach (var environment in envionments )
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

        public async Task<GetEnvironmentResponse> GetByIdAsync(int id)
        {
            _logger?.LogDebug("EnvironmentService::GetById() start!");
            _logger?.LogDebug("id = {Id}", id);

            try
            {
                var environment = await _dbContext.Environments
                    .Where(_ => _.Id == id)
                    .Include(_ =>_.Versions)
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
    }
}
