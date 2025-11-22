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
            return await _context.TestCases
                .Include(_ => _.TestLevel)
                .ToListAsync();
        }

        public async Task<ICollection<TestCase>> GetByIdAsync(int testLevelId)
        {
            return await _context.TestCases
                .Where(_ => _.TestLevelId == testLevelId)
                .Include(_ => _.TestLevel)
                .ToListAsync();
        }

        public async Task AddAsync(TestCase testCase)
        {
            TestLevel testLevel = _context.TestLevels.Find(testCase.TestLevelId)!;
            testCase.TestLevel = testLevel;
            _context.TestCases.Add(testCase);
            await _context.SaveChangesAsync();
        }
    }
}
