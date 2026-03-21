using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using TestManagement.API.Data;
using TestManagement.API.Data.Repositories;
using TestManagement.API.Features.TestCases.Create;
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
        public async Task<CreateTestCaseResponse> CreateVersionForExistingCaseAsync(CreateTestCaseRequest request, CancellationToken ct = default)
        {
            _logger?.LogDebug("TestCaseService::CreateVersionForExistingCaseAsync() start!");

            // This method is responsible for creating a new version for an existing test case.
            // It ensures that the new version is properly associated with the existing test case
            // and that all necessary data is provided.
            var testCase = await _context.TestCases
                .Where(_ => _.Code == request.Code)
                .Include(tc => tc.Versions)
                .FirstOrDefaultAsync(ct);
            if (testCase == null)
            {
                throw new InvalidOperationException($"TestCase {request.Code} not found.");
            }

            int retryCount = 0;
            while (retryCount < 3)
            {
                retryCount++;
                try
                {
                    using (var transaction = await _context.Database.BeginTransactionAsync(ct))
                    {
                        // Call the domain method TestCase.AddVersion (to ensure invariants within the model).
                        testCase.AddVersion(request.Name, request.Description, request.TestLevelId);
                        await _context.SaveChangesAsync(ct);

                        transaction.Commit();
                        break;
                    }
                }
                catch (DbUpdateException)
                {
                    _logger?.LogInformation($"Add new version of TestCase {request.Code} failed, retry.");
                }
            }

            var createdVersion = testCase.Versions.OrderByDescending(_ => _.VersionNumber).FirstOrDefault();
            if (null == createdVersion)
            {
                throw new Exception($"Failed to create new version of {request.Code}.");
            }
            var response = new CreateTestCaseResponse()
            {
                Id = createdVersion.Id,
                Code = testCase.Code,
                Name = createdVersion.Name,
                Description = createdVersion.Description,
                TestLevelId = createdVersion.TestLevelId,
                VersionNumber = createdVersion.VersionNumber
            };

            return response;
        }

        /// <summary>
        /// Creates a new test case along with an initial version using the provided request data.
        /// </summary>
        /// <param name="request">The request containing the code, name, description and test level id for the new test case.</param>
        /// <param name="ct">Cancellation token used to cancel the operation.</param>
        /// <returns>A <see cref="CreateTestCaseResponse"/> describing the created test case version.</returns>
        public async Task<CreateTestCaseResponse> CreateAsync(CreateTestCaseRequest request, CancellationToken ct = default)
        {
            _logger?.LogDebug("TestCaseService::CreateAsync(CreateTestCaseRequest) start!");

            var isExists = await _context.TestCases.AnyAsync(_ => _.Code == request.Code);
            if (isExists)
            {
                throw new Exception($"Test case with code {request.Code} already exists.");
            }
            var newTestCase = new TestCase()
            {
                Code = request.Code,
                IsActive = true
            };
            newTestCase.AddVersion(request.Name, request.Description, request.TestLevelId);
            _context.TestCases.Add(newTestCase);

            try
            {
                await _context.SaveChangesAsync(ct);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error occurred while saving new test case to the database.");
                throw;
            }

            var createdTestCase = newTestCase.Versions.OrderByDescending(_ => _.VersionNumber).FirstOrDefault();
            if (null == createdTestCase)
            {
                throw new Exception("Failed to create test case version.");
            }
            var response = new CreateTestCaseResponse()
            {
                Id = createdTestCase.Id,
                Code = newTestCase.Code,
                Name = createdTestCase.Name,
                Description = createdTestCase.Description,
                TestLevelId = createdTestCase.TestLevelId,
                VersionNumber = 1
            };

            return response;
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

        /// <summary>
        /// Processes a collection of create requests and attempts to create test cases for each request.
        /// Returns a per-request response indicating whether creation succeeded or failed.
        /// </summary>
        /// <param name="requests">Collection of create requests to process.</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>
        /// A collection of <see cref="CreateTestCaseResponse"/> objects. Successful entries will contain the created
        /// test case id and version information. Failed entries will return an object with Id = -1 and VersionNumber = 0.
        /// </returns>
        /// <remarks>
        /// The method attempts to create each request using <see cref="CreateWithoutSave"/> and accumulates the
        /// successful ones. A single call to <see cref="TestManagementDbContext.SaveChangesAsync"/> is executed for
        /// all successful creations. Exceptions during individual request processing do not abort the entire batch;
        /// failed requests are reported back in the returned collection.
        /// </remarks>
        public async Task<ICollection<CreateTestCaseResponse>> CreateAsync(ICollection<CreateTestCaseRequest> requests, CancellationToken ct = default)
        {
            _logger?.LogDebug("TestCaseService::Create(ICollection<CreateTestCaseRequest>) start!");

            var oks = new List<CreateTestCaseRequest>();
            var ngs = new List<CreateTestCaseRequest>();

            foreach (var request in requests)
            {
                try
                {
                    CreateWithoutSave(request);
                    oks.Add(request);
                }
                catch (Exception)
                {
                    ngs.Add(request);
                }
            }
            if (0 < oks.Count)
            {
                await _context.SaveChangesAsync(ct);
            }

            var responses = new List<CreateTestCaseResponse>();
            foreach (var ok in oks)
            {
                var testCase = _context.TestCases.Where(_ => _.Code == ok.Code).First();
                var response = new CreateTestCaseResponse()
                {
                    Id = testCase.Id,
                    Code = ok.Code,
                    Name = ok.Name,
                    Description = ok.Description,
                    TestLevelId = ok.TestLevelId,
                    VersionNumber = 1
                };
                responses.Add(response);
            }
            foreach (var ng in ngs)
            {
                var response = new CreateTestCaseResponse()
                {
                    Id = -1,
                    Code = ng.Code,
                    Name = ng.Name,
                    Description = ng.Description,
                    TestLevelId = ng.TestLevelId,
                    VersionNumber = 0
                };
                responses.Add(response);
            }
            return responses;
        }

        /// <summary>
        /// Prepares and adds a new <see cref="TestCase"/> with its initial version to the DbContext without persisting.
        /// </summary>
        /// <param name="request">Request containing the code, name, description and test level id for the new test case.</param>
        /// <remarks>
        /// This method performs a duplicate check based on the test case code and will throw an exception if a
        /// test case with the same code already exists. The created entity is added to the DbContext but this
        /// method does not call SaveChanges/SaveChangesAsync — the caller is responsible for persisting the unit of work.
        /// </remarks>
        public void CreateWithoutSave(CreateTestCaseRequest request)
        {
            var isExists = _context.TestCases.Any(_ => _.Code == request.Code);
            if (isExists)
            {
                string message = $"Test case with code {request.Code} already exists.";
                _logger?.LogError(message);
                throw new Exception(message);
            }

            var newTestCase = new TestCase()
            {
                Code = request.Code,
                IsActive = true
            };
            newTestCase.AddVersion(request.Name, request.Description, request.TestLevelId);
            _context.TestCases.Add(newTestCase);
        }


    }
}
