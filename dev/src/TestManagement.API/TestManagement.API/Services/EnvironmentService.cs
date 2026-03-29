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

            var envionments = await _dbContext.Environments.ToListAsync();
            var responses = new List<GetEnvironmentResponse>();
            foreach (var environment in envionments )
            {
                var response = new GetEnvironmentResponse
                {
                    Name = environment.Name,
                    Os = environment.Os ?? string.Empty,
                    RunTime = environment.RunTime ?? string.Empty
                };
                responses.Add(response);
            }

            return responses;
        }


    }
}
