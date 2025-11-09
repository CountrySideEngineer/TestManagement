using Microsoft.EntityFrameworkCore;
using TestManagement.Model;

namespace TestManagement.Data.Repositories
{
    public class ProjectRepository : GenericRepository<Project>, IProjectRepository
    {
        private readonly TestManagementDbContext _context;

        public ProjectRepository(TestManagementDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Project?> GetProjectWithSuitesAsync(int projectId)
        {
            return await _context.Projects
                .Include(_ => _.TestSuites)
                .ThenInclude(_ => _.TestCases)
                .FirstOrDefaultAsync(_ => _.Id == projectId);
        }
    }
}
