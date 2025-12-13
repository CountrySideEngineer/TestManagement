using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using TestManagement.API.Models;

namespace TestManagement.API.Data.Repositories
{
    public class TestRunRepository : ITestRunRepository
    {
        private readonly TestManagementDbContext _context;

        public TestRunRepository(TestManagementDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<TestRun>> GetAllAsync()
        {
            return await _context.TestRuns
                .Include(_ => _.TestResults)
                .ToListAsync();
        }

        public async Task<TestRun?> GetByIdAsync(int id)
        {
            return await _context.TestRuns
                .Include(_ => _.TestResults)
                .FirstAsync(_ => _.Id == id);
        }

        public async Task AddAsync(TestRun testRun)
        {
            _context.TestRuns.Add(testRun);
            await _context.SaveChangesAsync();
        }

        public async Task AddAsync(ICollection<TestRun> testRuns)
        {
            _context.TestRuns.AddRange(testRuns);
            await _context.SaveChangesAsync();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(TestRun testRun)
        {
            throw new NotImplementedException();
        }
    }
}
