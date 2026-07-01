using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using TestManagement.API.Data;
using TestManagement.API.Data.Repositories;
using TestManagement.API.Features.TestCases.Create;
using TestManagement.API.Features.TestCases.Get;
using TestManagement.API.Features.TestCases.Update;
using TestManagement.API.Models;
using static TestManagement.API.Features.TestCases.Get.GetTestCaseResponse;
namespace TestManagement.API.Services
{
    /// <summary>
    /// Service that provides use-case level operations for test cases and their versions.
    /// Acts as an application service that coordinates EF Core persistence via <see cref="TestManagementDbContext"/>.
    /// </summary>
    public class TestCaseService : ITestCaseService
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
        /// Creates a new instance of <see cref="TestCaseService"/>.
        /// </summary>
        /// <param name="context">The database context used for persistence and queries.</param>
        /// <param name="logger">Logger instance for diagnostic messages.</param>
        public TestCaseService(
            TestManagementDbContext context,
            ILogger<TestCaseService> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Load all TestCase entities including their Versions and map to GetTestCaseResponseWithVrersion DTOs.
        public virtual async Task<ICollection<GetTestCaseResponse>> GetAllAsync(CancellationToken ct)
        {
            _logger?.LogDebug("TestCaseService::GetAllAsync() start!");

            // Load all TestCases with their Versions in a no-tracking query to avoid tracking overhead.
            var testCases = await _context.TestCases
                .Include(tc => tc.Versions)
                .AsNoTracking()
                .ToListAsync();

            var responses = testCases.Select(tc => new GetTestCaseResponse
            {
                Id = tc.Id,
                Code = tc.Code,
                Versions = tc.Versions.Select(tcv => new GetTestCaseResponse.TestCaseVersionItem
                {
                    Id = tcv.Id,
                    Name = tcv.Name,
                    Description = tcv.Description,
                    VersionNumber = tcv.VersionNumber,
                    TestLevelId = tcv.TestLevelId,
                    IsLatest = tcv.IsLatest,
                    CreatedAt = tcv.CreatedAt,
                    UpdatedAt = tcv.UpdatedAt
                }).ToList()
            }).ToList();

            return responses;
        }


        /// <summary>
        /// Retrieves only the latest TestCaseVersion for each TestCase (the version with the highest VersionNumber).
        /// Projects the results to <see cref="GetTestCaseResponse"/> and includes associated TestLevel information if needed.
        /// Results are returned as no-tracking queries.
        /// </summary>
        /// <returns>
        /// A collection of <see cref="GetTestCaseResponse"/> objects representing the latest version per test case.
        /// </returns>
        public virtual async Task<ICollection<GetTestCaseResponse>> GetAllLatestVersionAsync(CancellationToken ct)
        {
            _logger?.LogDebug("TestCaseService::GetAllAsync() start!");

            var latestPerCase = _context.TestCaseVersions
                .GroupBy(v => v.TestCaseId)
                .Select(g => new { TestCaseId = g.Key, MaxVersion = g.Max(v => v.VersionNumber) });

            var query = from v in _context.TestCaseVersions
                        join m in latestPerCase on new { v.TestCaseId, v.VersionNumber } equals new { TestCaseId = m.TestCaseId, VersionNumber = m.MaxVersion }
                        join tc in _context.TestCases on v.TestCaseId equals tc.Id
                        select new GetTestCaseResponse
                        {
                            Id = tc.Id,
                            Code = tc.Code,
                            Versions = new List<GetTestCaseResponse.TestCaseVersionItem>
                            {
                                new GetTestCaseResponse.TestCaseVersionItem
                                {
                                    Id = v.Id,
                                    Name = v.Name,
                                    Description = v.Description,
                                    VersionNumber = v.VersionNumber,
                                    TestLevelId = v.TestLevelId,
                                    IsLatest = v.IsLatest,
                                    CreatedAt = v.CreatedAt,
                                    UpdatedAt = v.UpdatedAt
                                }
                            }
                        };

            return await query
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
        public virtual async Task<ICollection<TestCaseVersion>> GetByTestLevelIdAsync(int testLevelId, CancellationToken ct)
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
        public virtual async Task<GetTestCaseResponse> GetByTestCaseIdAsync(long testCaseId, CancellationToken ct)
        {
            _logger?.LogDebug("TestCaseService::GetByTestCaseIdAsync() start!");

            var testCase = await _context.TestCases
                .Where(_ => _.Id == testCaseId)
                .Include(_ => _.Versions)
                    .ThenInclude(_ => _.TestLevel)
                .FirstOrDefaultAsync();

            if (null == testCase)
            {
                return new GetTestCaseResponse();
            }

            var response = new GetTestCaseResponse
            {
                Id = testCase.Id,
                Code = testCase.Code,
                Versions = testCase.Versions.Select(tcv => new GetTestCaseResponse.TestCaseVersionItem
                {
                    Id = tcv.Id,
                    Name = tcv.Name,
                    Description = tcv.Description,
                    VersionNumber = tcv.VersionNumber,
                    TestLevelId = tcv.TestLevelId,
                    IsLatest = tcv.IsLatest,
                    CreatedAt = tcv.CreatedAt,
                    UpdatedAt = tcv.UpdatedAt
                }).ToList()
            };
            return response;
        }

        /// <summary>
        /// Retrieves a single <see cref="TestCaseVersion"/> by its primary key identifier.
        /// </summary>
        /// <param name="id">Identifier of the test case version.</param>
        /// <returns>The matching <see cref="TestCaseVersion"/> or null if not found.</returns>
        public virtual async Task<TestCaseVersion?> GetByVersionIdAsync(long id, CancellationToken ct)
        {
            _logger?.LogDebug("TestCaseService::GetByVersionIdAsync() start!");

            return await _context.TestCaseVersions
                .Where(v => v.Id == id)
                .Include(v => v.TestLevel)
                .Include(v => v.TestCase)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public virtual async Task<TestCaseVersion> GetLatestVersionByTestCaseIdAsync(long testCaseId, CancellationToken ct)
        {
            _logger?.LogDebug("TestCaseService::GetLatestVersionByTestCaseIdAsync() start!");

            try
            {
                var testCaseVersions = await _context.TestCaseVersions
                    .Where(_ => _.TestCaseId == testCaseId)
                    .OrderByDescending(_ => _.VersionNumber)
                    .Include(_ => _.TestLevel)
                    .AsNoTracking()
                    .FirstAsync();

                return testCaseVersions;
            }
            catch (Exception ex)
            when ((ex is ArgumentNullException) || (ex is InvalidOperationException))
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        /// <summary>
        /// Creates a new version for an existing test case identified by the request's code.
        /// Uses a retry loop and a transaction to handle concurrency when adding the version.
        /// </summary>
        /// <param name="request">Request containing the code, name, description and test level id for the new version.</param>
        /// <param name="ct">Cancellation token used to cancel the operation.</param>
        /// <returns>A <see cref="CreateTestCaseResponse"/> describing the newly created version.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the specified test case does not exist.</exception>
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

            // Load existing codes once to avoid N+1 queries
            var requestedCodes = requests.Select(r => r.Code).ToList();
            var existingCodes = new HashSet<string>(await _context.TestCases
                .Where(tc => requestedCodes.Contains(tc.Code))
                .Select(tc => tc.Code)
                .ToListAsync(ct));

            var notExistingTestCases = new List<CreateTestCaseRequest>();
            var existingTestCases = new List<CreateTestCaseRequest>();
            var newTestCases = new List<TestCase>();

            foreach (var request in requests)
            {
                if (existingCodes.Contains(request.Code))
                {
                    existingTestCases.Add(request);
                    continue;
                }

                // Prepare new entity without performing DB existence checks per item
                var newTestCase = new TestCase()
                {
                    Code = request.Code,
                    IsActive = true
                };
                newTestCase.AddVersion(request.Name, request.Description, request.TestLevelId);
                newTestCases.Add(newTestCase);
                notExistingTestCases.Add(request);

                // Prevent duplicates within the same batch
                existingCodes.Add(request.Code);
            }

            if (newTestCases.Count > 0)
            {
                _context.TestCases.AddRange(newTestCases);
                await _context.SaveChangesAsync(ct);
            }

            // Reload all test cases for the requested codes in a single query (includes newly created)
            var loadedTestCases = await _context.TestCases
                .Where(tc => requestedCodes.Contains(tc.Code))
                .Include(tc => tc.Versions)
                .ToListAsync(ct);
            var tcMap = loadedTestCases.ToDictionary(tc => tc.Code, tc => tc);

            var responses = new List<CreateTestCaseResponse>();
            foreach (var ok in notExistingTestCases)
            {
                if (!tcMap.TryGetValue(ok.Code, out var testCase))
                {
                    responses.Add(new CreateTestCaseResponse
                    {
                        Id = -1,
                        Code = ok.Code,
                        Name = ok.Name,
                        Description = ok.Description,
                        TestLevelId = ok.TestLevelId,
                        VersionNumber = 0
                    });
                    continue;
                }

                var createdTestCaseVersion = testCase.Versions.OrderByDescending(v => v.VersionNumber).FirstOrDefault();
                responses.Add(new CreateTestCaseResponse()
                {
                    Id = testCase.Id,
                    Code = ok.Code,
                    Name = createdTestCaseVersion?.Name ?? ok.Name,
                    Description = createdTestCaseVersion?.Description ?? ok.Description,
                    TestLevelId = createdTestCaseVersion?.TestLevelId ?? ok.TestLevelId,
                    VersionNumber = createdTestCaseVersion?.VersionNumber ?? 1
                });
            }

            foreach (var ng in existingTestCases)
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
        /// Ensures that each test case described in the provided requests exists in the database.
        /// For any request where a test case with the same code does not already exist, a new test case
        /// with an initial version will be prepared and persisted. Existing test cases are left unchanged.
        /// </summary>
        /// <param name="requests">Collection of create requests to process.</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>
        /// A collection of <see cref="CreateTestCaseResponse"/> objects representing the current
        /// latest version for each requested test case. Newly created items will contain the newly
        /// created version information; pre-existing items will return their current latest version.
        /// </returns>
        /// <remarks>
        /// This method performs existence checks and only adds new entities for codes that are not present.
        /// All created entities are persisted in a single SaveChanges call when any creations occur.
        /// </remarks>
        public async Task<ICollection<CreateTestCaseResponse>> CreateIfNotExistsAsync(ICollection<CreateTestCaseRequest> requests, CancellationToken ct = default)
        {
            _logger?.LogDebug("TestCaseService::CreateIfNotExistsAsync(ICollection<CreateTestCaseRequest> requests, CancellationToken tc) start!");

            // Prepare requested codes and load existing test cases once
            var requestedCodes = requests.Select(_ => _.Code).ToList();
            var existingTestCases = await _context.TestCases
                .Where(_ => requestedCodes.Contains(_.Code))
                .Include(_ => _.Versions)
                .ToListAsync(ct);

            var existingCodes = new HashSet<string>(existingTestCases.Select(tc => tc.Code));
            var newTestCases = new List<TestCase>();

            foreach (var request in requests)
            {
                if (existingCodes.Contains(request.Code))
                {
                    // Already exists, skip
                    continue;
                }

                var newTestCase = new TestCase()
                {
                    Code = request.Code,
                    IsActive = true
                };
                newTestCase.AddVersion(request.Name, request.Description, request.TestLevelId);
                newTestCases.Add(newTestCase);
                existingCodes.Add(request.Code); // prevent duplicates within the batch
            }

            if (newTestCases.Count > 0)
            {
                _context.TestCases.AddRange(newTestCases);
                await _context.SaveChangesAsync(ct);

                // Reload to include newly created entities and their versions
                existingTestCases = await _context.TestCases
                    .Where(tc => requestedCodes.Contains(tc.Code))
                    .Include(tc => tc.Versions)
                    .ToListAsync(ct);
            }

            var tcMap = existingTestCases.ToDictionary(tc => tc.Code, tc => tc);
            var responses = new List<CreateTestCaseResponse>();

            foreach (var request in requests)
            {
                if (!tcMap.TryGetValue(request.Code, out var testCase))
                {
                    // Shouldn't happen, but return a failed entry
                    responses.Add(new CreateTestCaseResponse
                    {
                        Id = -1,
                        Code = request.Code,
                        Name = request.Name,
                        Description = request.Description,
                        TestLevelId = request.TestLevelId,
                        VersionNumber = 0
                    });
                    continue;
                }

                var testCaseVersion = testCase.Versions.FirstOrDefault(v => v.IsLatest) ?? testCase.Versions.OrderByDescending(v => v.VersionNumber).FirstOrDefault();
                if (testCaseVersion == null)
                {
                    responses.Add(new CreateTestCaseResponse
                    {
                        Id = testCase.Id,
                        Code = testCase.Code,
                        Name = request.Name,
                        Description = request.Description,
                        TestLevelId = request.TestLevelId,
                        VersionNumber = 0
                    });
                    continue;
                }

                var response = new CreateTestCaseResponse()
                {
                    Id = testCase.Id,
                    Code = testCase.Code,
                    Name = testCaseVersion.Name,
                    Description = testCaseVersion.Description,
                    TestLevelId = testCaseVersion.TestLevelId,
                    VersionNumber = testCaseVersion.VersionNumber
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

        /// <summary>
        /// Updates a test case by creating a new version. If the request provides a new name or description
        /// those values are used; otherwise the latest version's values are reused.
        /// The method persists the new version and returns information about the created version.
        /// </summary>
        /// <param name="request">Request containing the test case code and optional name/description overrides.</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>
        /// An <see cref="UpdateTestCaseResponse"/> describing the newly created version after update.
        /// </returns>
        /// <exception cref="Exception">Thrown when the test case or its versions cannot be found.</exception>
        public async Task<UpdateTestCaseResponse> UpdateAsync(UpdateTestCaseRequest request, CancellationToken ct = default)
        {
            _logger?.LogDebug("TestCaseService::UpdateAsync(UpdateTestCaseRequest) start!");

            var isExists = _context.TestCases.Any(_ => _.Code == request.Code);
            if (!isExists)
            {
                string message = $"Test case with code {request.Code} does not exist.";
                _logger?.LogError(message);
                throw new Exception(message);
            }

            // Load the test case including its versions so we can determine the latest version number
            var testCase = await _context.TestCases
                .Where(_ => _.Code == request.Code)
                .Include(tc => tc.Versions)
                .FirstOrDefaultAsync(ct);
            if (testCase == null)
            {
                throw new Exception($"Test case with code {request.Code} not found after existence check.");
            }

            // Create a new version via the aggregate method
            var latestVersion = testCase.Versions
                .OrderByDescending(_ => _.VersionNumber)
                .FirstOrDefault();
            if (latestVersion == null)
            {
                throw new Exception($"No versions found for test case {request.Code}.");
            }

            string newName =
                ((string.IsNullOrEmpty(request.Name)) || (string.IsNullOrWhiteSpace(request.Name))) ?
                latestVersion.Name : request.Name;
            string newDescription =
                ((string.IsNullOrEmpty(request.Description)) || (string.IsNullOrWhiteSpace(request.Description))) ?
                latestVersion.Description : request.Description;
            testCase.AddVersion(newName, newDescription, latestVersion.TestLevelId);
            await _context.SaveChangesAsync(ct);

            testCase = await _context.TestCases
                .Where(_ => _.Code == request.Code)
                .FirstOrDefaultAsync(ct);
            if (testCase == null)
            {
                throw new Exception($"Test case with code {request.Code} not found after updating.");
            }

            var newTestCaseVersion = testCase.Versions.OrderByDescending(_ => _.VersionNumber).FirstOrDefault();
            if (newTestCaseVersion == null)
            {
                throw new Exception($"No versions found for test case {request.Code} after update.");
            }
            var response = new UpdateTestCaseResponse()
            {
                Code = request.Code,
                Name = newTestCaseVersion.Name,
                Description = newTestCaseVersion.Description,
                VersionNumber = newTestCaseVersion.VersionNumber
            };
            return response;
        }
    }
}
