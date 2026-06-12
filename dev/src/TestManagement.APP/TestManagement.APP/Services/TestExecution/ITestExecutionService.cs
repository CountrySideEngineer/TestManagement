using TestManagement.APP.Dto.TestExecution.Get;
using TestManagement.APP.Dto.TestExecution.Post;
using TestManagement.APP.ViewModel.Executions;

namespace TestManagement.APP.Services.TestExecution
{
    /// <summary>
    /// Interface for a service that manages test execution operations.
    /// Provides methods to retrieve and create test executions.
    /// </summary>
    public interface ITestExecutionService
    {
        /// <summary>
        /// Retrieves all test executions asynchronously.
        /// </summary>
        /// <returns>A collection of <see cref="ExecutionViewModel"/> objects, or null if the operation fails.</returns>
        Task<ICollection<ExecutionViewModel>?> GetExecutionsAsync();

        /// <summary>
        /// Retrieves all test executions with detailed information asynchronously.
        /// </summary>
        /// <returns>A collection of <see cref="GetTestExecutionResponse"/> objects, or null if the operation fails.</returns>
        Task<ICollection<GetTestExecutionResponse>?> GetTestExecutionsAsync();

        /// <summary>
        /// Retrieves a specific test execution by its identifier asynchronously.
        /// </summary>
        /// <param name="testExecutionId">The identifier of the test execution to retrieve.</param>
        /// <returns>An <see cref="ExecutionViewModel"/> representing the test execution, or null if not found.</returns>
        Task<ExecutionViewModel?> GetTestExecutionByIdAsync(long testExecutionId);

        /// <summary>
        /// Creates a new test execution asynchronously.
        /// </summary>
        /// <param name="executedAt">The date and time when the test execution occurred.</param>
        /// <param name="environment">The environment in which the test was executed.</param>
        /// <param name="revision">The revision or version identifier for the test execution.</param>
        /// <returns>A <see cref="PostTestExecutionResponse"/> containing the created test execution details, or null if creation fails.</returns>
        Task<PostTestExecutionResponse?> CreateExecutionAsync(DateTime executedAt, string environment, string revision);
    }
}
