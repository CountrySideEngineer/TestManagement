using TestManagement.APP.Dto.TestCase.Post;

namespace TestManagement.APP.ApiClients.TestResult
{
    public interface ITestResultApiClient
    {
        Task<IEnumerable<PostTestCaseResponse>> CreateTestResultAsync(
            IEnumerable<PostTestCaseRequest> requests,
            CancellationToken cancellationToken = default);
    }
}
