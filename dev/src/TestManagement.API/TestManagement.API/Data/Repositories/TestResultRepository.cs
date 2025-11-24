using Microsoft.EntityFrameworkCore;
using TestManagement.API.Models;

namespace TestManagement.API.Data.Repositories
{
    public class TestResultRepository : ITestResultRepository
    {
        private readonly TestManagementDbContext _context;

        public TestResultRepository(TestManagementDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<TestResult>> GetAllAsyc()
        {
            return await _context.TestResults
                .Include(_ => _.TestCase)
                .Include(_ => _.TestRun)
                .ToListAsync();
        }

        public async Task<TestResult> GetByIdAsync(int id)
        {
            return await _context.TestResults
                .Where(_ => _.Id == id)
                .Include(_ => _.TestCase)
                .Include(_ => _.TestRun)
                .FirstAsync();
        }

        public async Task AddAsync(TestResult result)
        {
            TestCase testCase = _context.TestCases.Find(result.TestCaseId) ?? throw new Exception();
            TestRun testRun = _context.TestRuns.Find(result.TestRunId) ?? throw new Exception();

            result.TestCase = testCase;
            result.TestRun = testRun;
            await _context.TestResults.AddAsync(result);
        }

        public async Task AddAsync(ICollection<TestResult> results)
        {
            foreach (var item in results)
            {
                TestCase testCase = _context.TestCases.Find(item.TestCaseId) ?? throw new Exception();
                TestRun testRun = _context.TestRuns.Find(item.TestRunId) ?? throw new Exception();
                item.TestCase = testCase;
                item.TestRun = testRun;
            }
            _context.TestResults.AddRange(results);
            await _context.SaveChangesAsync();
        }
    }
}
