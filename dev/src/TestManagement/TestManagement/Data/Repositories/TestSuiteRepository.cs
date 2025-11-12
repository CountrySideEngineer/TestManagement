using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using TestManagement.Model;

namespace TestManagement.Data.Repositories
{
    public class TestSuiteRepository
    {
        private readonly TestManagementDbContext _context;

        public TestSuiteRepository(TestManagementDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TestSuite>> GetAllAsync()
        {
            return await _context.TestSuites
                .Include(ts => ts.Project)
                .ToListAsync();
        }

        public async Task<TestSuite?> GetByIdAsync(int id)
        {
            return await _context.TestSuites
                .Include(ts => ts.Project)
                .FirstOrDefaultAsync(_ => _.Id == id);
        }

        public async Task AddAsync(TestSuite suite)
        {
            _context.TestSuites.Add(suite);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TestSuite suite)
        {
            _context.Entry(suite).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var suite = await _context.TestSuites.FindAsync(id);
            if (null != suite)
            {
                _context.TestSuites.Remove(suite);
                await _context.SaveChangesAsync();
            }
        }
    }
}
