using TestManagement.API.Features.Project.Get;

namespace TestManagement.API.Services
{
    public interface IProjectService
    {
        Task<ICollection<GetProjectResponse>> GetAllAsync(CancellationToken ct = default);

        Task<GetProjectResponse> GetByIdAsync(long id, CancellationToken ct = default);
    }
}
