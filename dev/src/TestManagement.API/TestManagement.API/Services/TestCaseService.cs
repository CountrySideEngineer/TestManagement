using Microsoft.EntityFrameworkCore;
using TestManagement.API.Data;
using TestManagement.API.Data.Repositories;
using TestManagement.API.Models;

namespace TestManagement.API.Services
{
    public class TestCaseService
    {
        private readonly TestManagementDbContext _context;
        private readonly ILogger<TestCaseService>? _logger = null;

        public TestCaseService(
            TestManagementDbContext context,
            ILogger<TestCaseService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public virtual async Task<ICollection<TestCaseVersion>> GetAllAsync()
        {
            _logger?.LogDebug("TestCaseService::GetAllAsync() start!");

            return await _context.TestCaseVersions
                .Include(_ => _.TestLevel)
                .AsNoTracking()
                .ToListAsync();
        }

        public virtual async Task<ICollection<TestCaseVersion>> GetByTestLevelIdAsync(int testLevelId)
        {
            _logger?.LogDebug("TestCaseService::GetByIdAsync() start!");

            return await _context.TestCaseVersions
                .Where(_ => _.TestLevelId == testLevelId)
                .Include(_ => _.TestLevel)
                .AsNoTracking()
                .ToListAsync();
        }

        public virtual async Task<ICollection<TestCaseVersion>> GetByTestCaseIdAsync(int testCaseId)
        {
            _logger?.LogDebug("TestCaseService::GetByTestCaseIdAsync() start!");

            return await _context.TestCaseVersions
                .Where(_ => _.TestCaseId == testCaseId)
                .Include(_ => _.TestLevel)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task CreateVersionForExistingCaseAsync(long testCaseId, string name, string description, int testLevelId, CancellationToken ct = default)
        {
            _logger?.LogDebug("TestCaseService::CreateVersionForExistingCaseAsync() start!");

            // This method is responsible for creating a new version for an existing test case.
            // It ensures that the new version is properly associated with the existing test case
            // and that all necessary data is provided.
            var testCase = await _context.TestCases
                .Include(tc => tc.Versions)
                .FirstOrDefaultAsync(tc => tc.Id == testCaseId, ct);

            if (testCase == null)
            {
                throw new InvalidOperationException($"TestCase {testCaseId} not found.");
            }


            // Call the domain method TestCase.AddVersion (to ensure invariants within the model).
            testCase.AddVersion(name, description, testLevelId);

            await _context.SaveChangesAsync(ct);
        }

        public async Task CreateAsync(string name, string description, int testLevelId, CancellationToken ct = default)
        {
            _logger?.LogDebug("TestCaseService::CreateAsync() start!");

            var testCaseCount = await _context.TestCaseVersions
                .Where(_ => _.Name == name && _.Description == description && _.TestLevelId == testLevelId)
                .CountAsync(ct);
            if (0 < testCaseCount)
            {
                throw new InvalidOperationException($"Test case named \"{name}\" has already been registerd.");
            }

            var newTestCaseVersion = new TestCaseVersion()
            {
                Name = name,
                Description = description,
                TestLevelId = testLevelId,
                VersionNumber = 1
            };
            _context.TestCaseVersions.Add(newTestCaseVersion);

            await _context.SaveChangesAsync();
        }

        public async Task CreateAsync(TestCaseVersion testCaseVersion, CancellationToken ct = default)
        {
            _logger?.LogDebug("TestCaseService::CreateAsync(TestCaseVersion) start!");

            _context.TestCaseVersions.Add(testCaseVersion);
            await _context.SaveChangesAsync(ct);
        }

        public async Task CreateAsync(ICollection<TestCaseVersion> testCases, CancellationToken ct = default)
        {
            _logger?.LogDebug("TestCaseService::Create(ICollection<TestCaseVersion>) start!");

            _context.TestCaseVersions.AddRange(testCases);
            await _context.SaveChangesAsync(ct);
        }
    }
}
