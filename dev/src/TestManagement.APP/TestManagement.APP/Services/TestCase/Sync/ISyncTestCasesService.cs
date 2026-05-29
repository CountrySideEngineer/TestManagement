using TestManagement.APP.Dto.TestCase.Post;
using TestManagement.APP.Dto.TestResult;

namespace TestManagement.APP.Services.TestCase.Sync
{
    public interface ISyncTestCasesService
    {
        /// <summary>
        /// Synchronizes test cases with the backend by sending a collection of <see cref="SyncTestCaseRequest"/> to the API.
        /// </summary>
        /// <param name="syncTestCaseRequests">The collection of test case requests to synchronize.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task SyncTestCasesAsync(IEnumerable<ParsedTestResult> syncTestCaseRequests);
    }
}
