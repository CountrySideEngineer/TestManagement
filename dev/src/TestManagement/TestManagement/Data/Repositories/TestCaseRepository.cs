using Microsoft.EntityFrameworkCore;
using TestManagement.Model;

namespace TestManagement.Data.Repositories
{
    public class TestCaseRepository : ITestCaseRepository
    {
        private readonly TestManagementDbContext _context;

        public TestCaseRepository(TestManagementDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TestCase>> GetAllAsync()
        {
            return await _context.TestCases
                .Include(_ => _.TestSuite)
                .Include(_ => _.TestLevel)
                .Include(_ => _.TestResults)
                .ToListAsync();
        }

        public async Task<IEnumerable<TestCase>> GetByTestSuiteIdAsync(int testSuiteId)
        {
            return await _context.TestCases
                .Where(_ => _.TestSuiteId == testSuiteId)
                .Include(_ => _.TestSuite)
                .Include(_ => _.TestLevel)
                .Include(_ => _.TestResults)
                .ToListAsync();
        }

        public async Task<TestCase?> GetByIdAsync(int id)
        {
            return await _context.TestCases
                .Include(_ => _.TestSuite)
                .Include(_ => _.TestLevel)
                .Include(_ => _.TestResults)
                .FirstOrDefaultAsync(_ => _.Id == id);
        }

        public async Task AddAsync(TestCase testCase)
        {
            _context.TestCases.Add(testCase);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TestCase testCase)
        {
            _context.Entry(testCase).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var testCase = await _context.TestCases.FindAsync(id);
            if (null != testCase)
            {
                _context.TestCases.Remove(testCase);
                await _context.SaveChangesAsync();
            }
        }
    }
}
