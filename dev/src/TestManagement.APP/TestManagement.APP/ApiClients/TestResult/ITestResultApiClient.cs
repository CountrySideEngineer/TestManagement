using TestManagement.APP.Dto.TestCase.Post;
using TestManagement.APP.Dto.TestResult.Post;

namespace TestManagement.APP.ApiClients.TestResult
{
    public interface ITestResultApiClient
    {
        Task<IEnumerable<PostTestResultResponse>> CreateTestResultAsync(
            IEnumerable<PostTestResultRequest> requests,
            CancellationToken cancellationToken = default);
    }
}
