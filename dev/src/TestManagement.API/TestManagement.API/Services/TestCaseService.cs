using Microsoft.EntityFrameworkCore;
using TestManagement.API.Data;
using TestManagement.API.Data.Repositories;

using TestManagement.API.Models;
namespace TestManagement.API.Services
{
    /// <summary>
    /// Service that provides use-case level operations for test cases and their versions.
    /// Acts as an application service that coordinates EF Core persistence via <see cref="TestManagementDbContext"/>.
    /// </summary>
    public class TestCaseService
    {
        /// <summary>
        /// EF Core DbContext used for data access and unit-of-work operations.
        /// </summary>
        private readonly TestManagementDbContext _context;

        /// <summary>
        /// Logger instance for diagnostic messages. Nullable to allow optional injection in some test scenarios.
        /// </summary>
        private readonly ILogger<TestCaseService>? _logger = null;

        /// <summary>
        /// Constructs a new instance of <see cref="TestCaseService"/>.
        /// </summary>
        /// <param name="context">The <see cref="TestManagementDbContext"/> used for persistence.</param>
        /// <param name="logger">The logger used for diagnostic output.</param>
        public TestCaseService(
            TestManagementDbContext context,
            ILogger<TestCaseService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all test case versions including their associated test level information.
        /// </summary>
        /// <returns>
        /// A collection of <see cref="TestCaseVersion"/> objects. The results are returned as no-tracking queries.
        /// </returns>
        public virtual async Task<ICollection<TestCaseVersion>> GetAllAsync()
        {
            _logger?.LogDebug("TestCaseService::GetAllAsync() start!");

            return await _context.TestCaseVersions
                .Include(_ => _.TestLevel)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves all test case versions that belong to the specified test level.
        /// </summary>
        /// <param name="testLevelId">Identifier of the test level to filter by.</param>
        /// <returns>
        /// A collection of <see cref="TestCaseVersion"/> that match the specified test level.
        /// </returns>
        public virtual async Task<ICollection<TestCaseVersion>> GetByTestLevelIdAsync(int testLevelId)
        {
            _logger?.LogDebug("TestCaseService::GetByIdAsync() start!");

            return await _context.TestCaseVersions
                .Where(_ => _.TestLevelId == testLevelId)
                .Include(_ => _.TestLevel)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves all versions for a specific test case.
        /// </summary>
        /// <param name="testCaseId">Identifier of the test case whose versions will be returned.</param>
        /// <returns>
        /// A collection of <see cref="TestCaseVersion"/> for the specified test case id.
        /// </returns>
        public virtual async Task<ICollection<TestCaseVersion>> GetByTestCaseIdAsync(int testCaseId)
        {
            _logger?.LogDebug("TestCaseService::GetByTestCaseIdAsync() start!");

            return await _context.TestCaseVersions
                .Where(_ => _.TestCaseId == testCaseId)
                .Include(_ => _.TestLevel)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Creates a new version and associates it with an existing <see cref="Models.TestCase"/>.
        /// </summary>
        /// <param name="testCaseId">The id of the existing test case to which the version will be added.</param>
        /// <param name="name">The name/title of the new version.</param>
        /// <param name="description">A textual description of the new version.</param>
        /// <param name="testLevelId">The id of the test level associated with the new version.</param>
        /// <param name="ct">Cancellation token used to cancel the operation.</param>
        /// <exception cref="InvalidOperationException">Thrown when the specified test case does not exist.</exception>
        /// <remarks>
        /// This method loads the aggregate (<see cref="Models.TestCase"/>), calls its domain method
        /// to add a version (so invariants are applied inside the model), then saves the unit of work.
        /// </remarks>
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

        /// <summary>
        /// Creates a standalone test case version. This method performs a simple duplicate check
        /// on name/description/testLevel and then inserts a new <see cref="TestCaseVersion"/>.
        /// </summary>
        /// <param name="name">The name/title of the test case version to create.</param>
        /// <param name="description">A textual description for the new version.</param>
        /// <param name="testLevelId">The id of the test level associated with the new version.</param>
        /// <param name="ct">Cancellation token used to cancel the operation.</param>
        /// <exception cref="InvalidOperationException">Thrown when a test case with the same name/description/test level already exists.</exception>
        /// <remarks>
        /// Note: This method creates a TestCaseVersion without attaching it to an existing TestCase aggregate.
        /// Consider using <see cref="CreateVersionForExistingCaseAsync"/> if you need to maintain aggregate invariants.
        /// </remarks>
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

        /// <summary>
        /// Adds the provided <see cref="TestCaseVersion"/> to the database directly.
        /// </summary>
        /// <param name="testCaseVersion">The test case version entity to add.</param>
        /// <param name="ct">Cancellation token used to cancel the operation.</param>
        /// <remarks>
        /// Directly adding a <see cref="TestCaseVersion"/> may bypass domain invariants enforced on aggregates.
        /// Use with caution when aggregate rules must be preserved.
        /// </remarks>
        public async Task CreateAsync(TestCaseVersion testCaseVersion, CancellationToken ct = default)
        {
            _logger?.LogDebug("TestCaseService::CreateAsync(TestCaseVersion) start!");

            _context.TestCaseVersions.Add(testCaseVersion);
            await _context.SaveChangesAsync(ct);
        }

        /// <summary>
        /// Adds a collection of <see cref="TestCaseVersion"/> entities to the database.
        /// </summary>
        /// <param name="testCases">The collection of test case versions to add.</param>
        /// <param name="ct">Cancellation token used to cancel the operation.</param>
        /// <remarks>
        /// Bulk insertion can be more efficient but may still bypass aggregate-level invariants.
        /// Consider domain-level validation before calling this method.
        /// </remarks>
        public async Task CreateAsync(ICollection<TestCaseVersion> testCases, CancellationToken ct = default)
        {
            _logger?.LogDebug("TestCaseService::Create(ICollection<TestCaseVersion>) start!");

            _context.TestCaseVersions.AddRange(testCases);
            await _context.SaveChangesAsync(ct);
        }
    }
}
