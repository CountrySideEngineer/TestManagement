using TestManagement.Data.Repositories;
using TestManagement.Model;

namespace TestManagement.Serivces
{
    public class ProjectService
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<IEnumerable<Project>> GetAllAsync()
        {
            return await _projectRepository.GetAllAsync();
        }

        public async Task<Project?> GetProjectWithSuiteAsync(int projectId)
        {
            return await _projectRepository.GetProjectWithSuitesAsync(projectId);
        }

        public async Task AddAsync(Project project)
        {
            await _projectRepository.AddAsync(project);
            await _projectRepository.SaveChangesAsync();
        }
    }
}
