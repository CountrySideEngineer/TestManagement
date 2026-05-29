using TestManagement.APP.Dto.TestCase.Post;

namespace TestManagement.APP.ApiClients.TestCase
{
    public interface ITestCaseSyncApiClient
    {
        Task<IEnumerable<PostTestCaseResponse>> SyncAsync(IEnumerable<PostTestCaseRequest> requests, CancellationToken cancellationToken = default);
    }
}
