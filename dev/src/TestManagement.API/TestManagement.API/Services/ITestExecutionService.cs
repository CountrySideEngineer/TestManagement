using TestManagement.API.Features.TestExecutions.Create;
using TestManagement.API.Features.TestExecutions.Get;
using TestManagement.API.Features.TestExecutions.Update;

namespace TestManagement.API.Services
{
    /// <summary>
    /// Service responsible for operations related to test executions.
    /// This includes querying, creating and updating execution records.
    /// </summary>
    public interface ITestExecutionService
    {
        /// <summary>
        /// Get all test executions.
        /// </summary>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>A list of <see cref="GetTestExecutionResponse"/> objects.</returns>
        Task<List<GetTestExecutionResponse>> GetAsync(CancellationToken ct);

        /// <summary>
        /// Get a single test execution by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the test execution.</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>The <see cref="GetTestExecutionResponse"/> for the given id.</returns>
        Task<GetTestExecutionResponse> GetByIdAsync(long id, CancellationToken ct);

        /// <summary>
        /// Create a new test execution record.
        /// </summary>
        /// <param name="request">The creation request containing execution details.</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>The created execution information wrapped in <see cref="CreateTestExecutionResponse"/>.</returns>
        Task<CreateTestExecutionResponse> CreateAsync(CreateTestExecutionRequest request, CancellationToken ct);

        /// <summary>
        /// Update an existing test execution.
        /// </summary>
        /// <param name="request">The update request containing modified execution details.</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>The updated execution information wrapped in <see cref="UpdateTestExecutionResponse"/>.</returns>
        Task<UpdateTestExecutionResponse> UpdateAsync(UpdateTestExecutionRequest request, CancellationToken ct);
    }
}
