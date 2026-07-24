using TestManagement.API.Features.TestResult.Create;
using TestManagement.API.Features.TestResult.Get;

namespace TestManagement.API.Services
{
    /// <summary>
    /// Service responsible for managing test results.
    /// Provides retrieval and creation operations for test result records.
    /// </summary>
    public interface ITestResultService
    {
        /// <summary>
        /// Retrieve all test results.
        /// </summary>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>A collection of <see cref="GetTestResultResponse"/> objects.</returns>
        Task<ICollection<GetTestResultResponse>> GetAllAsync(CancellationToken ct);

        /// <summary>
        /// Retrieve a specific test result by identifier.
        /// </summary>
        /// <param name="id">The identifier of the test result.</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>The <see cref="GetTestResultResponse"/> for the given id.</returns>
        Task<GetTestResultResponse> GetByIdAsync(int id, CancellationToken ct);

        /// <summary>
        /// Create a new test result record.
        /// </summary>
        /// <param name="request">Creation request containing test result details.</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>The created test result information wrapped in <see cref="CreateTestResultResponse"/>.</returns>
        Task<CreateTestResultResponse> CreateAsync(CreateTestResultRequest request, CancellationToken ct);

        /// <summary>
        /// Create multiple test results in batch.
        /// </summary>
        /// <param name="requests">Collection of creation requests.</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>Collection of created test result responses.</returns>
        Task<ICollection<CreateTestResultResponse>> CreateAsync(ICollection<CreateTestResultRequest> requests, CancellationToken ct);
    }
}
