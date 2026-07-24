using TestManagement.API.Features.TestCases.Create;
using TestManagement.API.Features.TestCases.Get;
using TestManagement.API.Features.TestCases.Update;
using TestManagement.API.Models;

namespace TestManagement.API.Services
{
    /// <summary>
    /// Service for managing test cases and their versions.
    /// Provides methods for querying, creating and updating test case entities and versions.
    /// </summary>
    public interface ITestCaseService
    {
        /// <summary>
        /// Retrieve all test cases (all versions included).
        /// </summary>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>Collection of <see cref="GetTestCaseResponse"/> objects.</returns>
        Task<ICollection<GetTestCaseResponse>> GetAllAsync(CancellationToken ct);

        /// <summary>
        /// Retrieve only the latest version for each test case.
        /// </summary>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>Collection of <see cref="GetTestCaseResponse"/> representing latest versions.</returns>
        Task<ICollection<GetTestCaseResponse>> GetAllLatestVersionAsync(CancellationToken ct);

        /// <summary>
        /// Get test case versions that belong to a specific test level.
        /// </summary>
        /// <param name="testLevelId">Identifier of the test level.</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>Collection of <see cref="TestCaseVersion"/> for the given level.</returns>
        Task<ICollection<TestCaseVersion>> GetByTestLevelIdAsync(int testLevelId, CancellationToken ct);

        /// <summary>
        /// Get the test case (latest view) by the test case identifier.
        /// </summary>
        /// <param name="testCaseId">Identifier of the test case.</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>A <see cref="GetTestCaseResponse"/> representing the test case.</returns>
        Task<GetTestCaseResponse> GetByTestCaseIdAsync(long testCaseId, CancellationToken ct);

        /// <summary>
        /// Get a specific test case version by version id.
        /// </summary>
        /// <param name="id">Version identifier.</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>The <see cref="TestCaseVersion"/> if found; otherwise null.</returns>
        Task<TestCaseVersion?> GetByVersionIdAsync(long id, CancellationToken ct);

        /// <summary>
        /// Retrieve the latest version of a given test case by its identifier.
        /// </summary>
        /// <param name="testCaseId">Identifier of the test case.</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>The latest <see cref="TestCaseVersion"/> for the test case.</returns>
        Task<TestCaseVersion> GetLatestVersionByTestCaseIdAsync(long testCaseId, CancellationToken ct);

        /// <summary>
        /// Create a new version for an existing test case.
        /// </summary>
        /// <param name="request">Creation request containing new version details.</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>The created version wrapped in <see cref="CreateTestCaseResponse"/>.</returns>
        Task<CreateTestCaseResponse> CreateVersionForExistingCaseAsync(CreateTestCaseRequest request, CancellationToken ct);

        /// <summary>
        /// Create a new test case (initial version).
        /// </summary>
        /// <param name="request">Creation request containing test case details.</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>The created test case information.</returns>
        Task<CreateTestCaseResponse> CreateAsync(CreateTestCaseRequest request, CancellationToken ct);

        /// <summary>
        /// Create multiple test cases in batch.
        /// </summary>
        /// <param name="requests">Collection of creation requests.</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>Collection of created test case responses.</returns>
        Task<ICollection<CreateTestCaseResponse>> CreateAsync(ICollection<CreateTestCaseRequest> requests, CancellationToken ct);

        /// <summary>
        /// Create test cases only if they do not already exist.
        /// </summary>
        /// <param name="requests">Collection of creation requests.</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>Collection of created test case responses for newly created items.</returns>
        Task<ICollection<CreateTestCaseResponse>> CreateIfNotExistsAsync(ICollection<CreateTestCaseRequest> requests, CancellationToken ct);

        /// <summary>
        /// Update an existing test case or its version.
        /// </summary>
        /// <param name="request">Update request containing modified fields.</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>The updated test case response.</returns>
        Task<UpdateTestCaseResponse> UpdateAsync(UpdateTestCaseRequest request, CancellationToken ct);
    }
}
