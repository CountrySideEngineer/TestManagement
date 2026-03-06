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
            throw new NotSupportedException();
        }

        public async Task<TestRun?> GetByIdAsync(int id)
        {
            throw new NotSupportedException();
        }

        public async Task<TestRun> AddAsync(TestRun testRun)
        {
            throw new NotSupportedException();
        }

        public async Task AddAsync(ICollection<TestRun> testRuns)
        {
            throw new NotSupportedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotSupportedException();
        }

        public Task UpdateAsync(TestRun testRun)
        {
            throw new NotSupportedException();
        }
    }
}
