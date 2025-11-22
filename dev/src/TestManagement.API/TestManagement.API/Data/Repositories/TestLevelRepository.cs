using Microsoft.EntityFrameworkCore;
using TestManagement.API.Models;

namespace TestManagement.API.Data.Repositories
{
    public class TestLevelRepository : ITestLevelRepository
    {
        private readonly TestManagementDbContext _context;

        public TestLevelRepository(TestManagementDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<TestLevel>> GetAllAsync()
        {
            return await _context.TestLevels.ToListAsync();
        }
    }
}
