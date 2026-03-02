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

        public async Task<ICollection<TestCaseVersion>> GetAllAsync()
        {
            return await _context.TestCases
                .Include(_ => _.TestLevel)
                .ToListAsync();
        }

        public async Task<ICollection<TestCaseVersion>> GetByIdAsync(int id)
        {
            return await _context.TestCases
                .Where(_ => _.Id == id)
                .Include(_ => _.TestLevel)
                .ToListAsync();
        }

        public async Task AddAsync(TestCaseVersion testCase)
        {
            TestLevel testLevel = _context.TestLevels.Find(testCase.TestLevelId)!;
            testCase.TestLevel = testLevel;
            _context.TestCases.Add(testCase);
            await _context.SaveChangesAsync();
        }

        public async Task AddAsync(ICollection<TestCaseVersion> testCases)
        {
            foreach (var item in testCases)
            {
                TestLevel testLevel = _context.TestLevels.Find(item.TestLevelId)!;
                item.TestLevel = testLevel;
            }
            _context.TestCases.AddRange(testCases);
            await _context.SaveChangesAsync();
        }
    }
}
