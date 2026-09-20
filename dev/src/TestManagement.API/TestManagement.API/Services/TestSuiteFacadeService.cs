using System.Runtime.CompilerServices;
using TestManagement.API.Features.TestSuite.Create;
using TestManagement.API.Features.TestSuite.Get;
using TestManagement.API.Models;

namespace TestManagement.API.Services;

/// <summary>
/// Facade service that orchestrates test suite related operations by delegating
/// to lower-level services. Provides a simplified API for retrieving and
/// manipulating test suites and their compositions.
/// </summary>
public class TestSuiteFacadeService : ITestSuiteFacadeService
{
    /// <summary>
    /// Service responsible for core test suite operations (retrieve, persist, etc.).
    /// </summary>
    private readonly ITestSuiteService _testSuiteService;

    /// <summary>
    /// Service responsible for handling test suite composition (relationships between test suites and test cases).
    /// </summary>
    private readonly ITestSuiteCompositionService _testSuiteCompositionService;

    /// <summary>
    /// Logger instance for logging diagnostic and operational information.
    /// </summary>
    private readonly ILogger<TestSuiteFacadeService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestSuiteFacadeService"/> class.
    /// </summary>
    /// <param name="testSuiteService">The underlying test suite service used for core operations.</param>
    /// <param name="testSuiteCompositionService">The service used to manage test suite compositions.</param>
    /// <param name="logger">Logger used for diagnostics and tracing.</param>
    public TestSuiteFacadeService(
        ITestSuiteService testSuiteService,
        ITestSuiteCompositionService testSuiteCompositionService,
        ILogger<TestSuiteFacadeService> logger)
    {
        _testSuiteService = testSuiteService;
        _testSuiteCompositionService = testSuiteCompositionService;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves all test suites.
    /// </summary>
    /// <param name="ct">A <see cref="CancellationToken"/> to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of <see cref="GetTestSuiteResponse"/>.</returns>
    public Task<ICollection<GetTestSuiteResponse>> GetAllAsync(CancellationToken ct = default)
    {
        _logger?.LogDebug("TestSuiteFacadeService::GetAllAsync start");

        var testSuites = _testSuiteService.GetAllAsync(ct);

        return testSuites;
    }

    /// <summary>
    /// Retrieves a single test suite by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the test suite to retrieve.</param>
    /// <param name="ct">A <see cref="CancellationToken"/> to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="GetTestSuiteResponse"/> for the specified id.</returns>
    public async Task<GetTestSuiteResponse> GetByIdAsync(long id, CancellationToken ct = default)
    {
        _logger?.LogDebug("TestSuiteFacadeService::GetByIdAsync start");

        var testSuite = await _testSuiteService.GetByIdAsync(id, ct);

        return testSuite;
    }

    /// <summary>
    /// Retrieve a test suite along with its included test case summaries.
    /// </summary>
    /// <remarks>
    /// - Loads the basic test suite DTO via <see cref="_testSuiteService.GetByIdAsync(long, CancellationToken)"/>.
    /// - Loads related suite compositions via <see cref="_testSuiteCompositionService.GetByTestSuiteId(long, CancellationToken)"/>.
    /// - Projects each composition's <c>TestCaseVersion</c> into <see cref="GetTestSuiteResponse.TestCaseSummary"/>.
    /// - If the suite is not found, an empty <see cref="GetTestSuiteResponse"/> instance is returned.
    /// - Exceptions from underlying services are not swallowed and will propagate to the caller.
    /// - Consider projecting only required fields in the composition query to avoid N+1 and reduce memory usage.
    /// </remarks>
    /// <param name="id">Primary key identifier of the test suite to retrieve.</param>
    /// <param name="ct">Cancellation token to cancel the operation.</param>
    /// <returns>
    /// A <see cref="GetTestSuiteResponse"/> containing suite metadata and a collection of test case summaries.
    /// If no suite is found, an empty response instance is returned.
    /// </returns>
    public async Task<GetTestSuiteWithTestCaseResponse> GetByIdWithTestCasesAsync(long id, CancellationToken ct = default)
    {
        _logger?.LogDebug("TestSuiteFacadeService::GetByIdWithTestCasesAsync start");

        var testSuiteWithTestCase = new GetTestSuiteWithTestCaseResponse();
        var testSuite = await _testSuiteService.GetByIdAsync(id, ct);
        if (testSuite is null)
        {
            return testSuiteWithTestCase;
        }
        testSuiteWithTestCase.Id = testSuite.Id;
        testSuiteWithTestCase.Name = testSuite.Name;
        testSuiteWithTestCase.Description = testSuite.Description;
        testSuiteWithTestCase.TestCaseSummaries = new List<GetTestSuiteWithTestCaseResponse.TestCaseSummary>();

        var compositions = await _testSuiteCompositionService.GetByTestSuiteId(id, ct);
        if (compositions is null)
        {
            return testSuiteWithTestCase;
        }

        var summaries = compositions
            .Select(tcv => new GetTestSuiteWithTestCaseResponse.TestCaseSummary()
            {
                Id = tcv.TestCaseVersionId,
                VersionNumber = tcv.TestCaseVersion?.VersionNumber ?? 0,
                Name = tcv.TestCaseVersion?.Name ?? string.Empty,
                Description = tcv.TestCaseVersion?.Description ?? string.Empty,
                TestLevelName = tcv.TestCaseVersion?.TestLevel?.DisplayName ?? string.Empty,
            })
            .ToList();
        testSuiteWithTestCase.TestCaseSummaries = summaries;

        return testSuiteWithTestCase;
    }

    /// <summary>
    /// Creates a new test suite by delegating the request to the underlying test suite service.
    /// </summary>
    /// <param name="request">The request containing details required to create the test suite.</param>
    /// <param name="ct">A <see cref="CancellationToken"/> to cancel the operation.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains
    /// a <see cref="CreateTestSuiteResponse"/> describing the created test suite.
    /// </returns>
    public async Task<CreateTestSuiteResponse> CreateAsync(CreateTestSuiteRequest request, CancellationToken ct = default)
    {
        _logger?.LogDebug("TestSuiteFacadeService::CreateAsync start");

        var response = await _testSuiteService.CreateAsync(request, ct);

        return response;
    }

    /// <summary>
    /// Creates a composition that links a specific test case version to a test suite by delegating
    /// to the underlying <see cref="ITestSuiteCompositionService"/>. If the composition cannot be created
    /// because the referenced test case version does not exist, an empty <see cref="CreateTestSuiteCompositionResponse"/>
    /// is returned.
    /// </summary>
    /// <param name="request">Request containing the target test suite id, test case id and version number.</param>
    /// <param name="ct">Cancellation token to cancel the asynchronous operation.</param>
    /// <returns>
    /// A <see cref="CreateTestSuiteCompositionResponse"/> describing the existing or newly created composition.
    /// Returns an empty response object when the operation could not be completed.
    /// </returns>
    public async Task<CreateTestSuiteCompositionResponse> CreateCompositionAsync(CreateTestSuiteCompositionRequest request, CancellationToken ct = default)
    {
        _logger?.LogDebug("TestSuiteFacadeService::CreateCompositionAsync start");

        var response = await _testSuiteCompositionService.CreateCompositionAsync(request, ct);
        if (response is null)
        {
            return new CreateTestSuiteCompositionResponse();
        }
        else
        {
            return response;
        }
    }

    /// <summary>
    /// Creates multiple compositions that link test case versions to test suites by delegating
    /// the work to the underlying <see cref="ITestSuiteCompositionService"/>.
    /// If the delegated service returns <c>null</c>, this facade will return an empty collection.
    /// Each request in the provided collection is processed independently; requests referencing
    /// non-existent test case versions are effectively skipped by the delegated service.
    /// </summary>
    /// <param name="requests">A collection of <see cref="CreateTestSuiteCompositionRequest"/> specifying test suites, test cases and version numbers to link.</param>
    /// <param name="ct">Cancellation token to cancel the asynchronous operation.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a collection
    /// of <see cref="CreateTestSuiteCompositionResponse"/> describing existing or newly created compositions.
    /// The returned collection will never be <c>null</c> (an empty list is returned when there are no successful compositions).
    /// </returns>
    public async Task<ICollection<CreateTestSuiteCompositionResponse>> CreateCompositionsAsync(ICollection<CreateTestSuiteCompositionRequest> requests, CancellationToken ct = default)
    {
        _logger?.LogDebug("TestSuiteFacadeService::CreateCompositionsAsync start");
        var responses = await _testSuiteCompositionService.CreateCompositionsAsync(requests, ct);
        if (responses is null)
        {
            return new List<CreateTestSuiteCompositionResponse>();
        }
        else
        {
            return responses;
        }
    }
}
