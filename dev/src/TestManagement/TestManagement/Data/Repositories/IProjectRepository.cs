using TestManagement.Model;

namespace TestManagement.Data.Repositories
{
    public interface IProjectRepository : IGenericRepository<Project>
    {
        Task<Project?> GetProjectWithSuitesAsync(int projectId);
    }
}
