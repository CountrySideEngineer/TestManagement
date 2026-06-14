using TestManagement.APP.Dto.TestResult.Import;

namespace TestManagement.APP.Services.TestExecution.Import
{
    /// <summary>
    /// Interface for a service that imports test results from various sources.
    /// </summary>
    public interface IImportTestResultService
    {
        /// <summary>
        /// Imports test results from the provided source asynchronously.
        /// </summary>
        /// <param name="execId">Identifier of the test execution.</param>
        /// <param name="execItemId">Identifier of the test execution item.</param>
        /// <param name="testId">Identifier of the test level.</param>
        /// <param name="request">Import request containing the source and parser.</param>
        /// <param name="ct">Cancellation token for the asynchronous operation.</param>
        /// <returns>An <see cref="ImportTestResultResponse"/> indicating the result of the import.</returns>
        Task<IEnumerable<ImportTestResultResponse>?> ImportAsync(
            long execId,
            long execItemId,
            long testId,
            ImportTestResultRequest request,
            CancellationToken ct = default);
    }
}
