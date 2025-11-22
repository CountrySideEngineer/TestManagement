using Microsoft.EntityFrameworkCore;
using TestManagement.API.Models;

namespace TestManagement.API.Data.Repositories
{
    public class TestCaseRepository : ITestCaseRepository
    {
        private readonly TestManagementDbContext _context;

        public TestCaseRepository(TestManagementDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<TestCase>> GetAllAsync()
        {
            return await _context.TestCases.ToListAsync();
        }

        public async Task<ICollection<TestCase>> GetByIdAsync(int testLevelId)
        {
            return await _context.TestCases
                .Where(_ => _.TestLevelId == testLevelId)
                .ToListAsync();
        }
    }
}
