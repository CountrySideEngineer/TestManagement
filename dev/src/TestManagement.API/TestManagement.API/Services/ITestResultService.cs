using TestManagement.API.Features.TestResult.Create;
using TestManagement.API.Features.TestResult.Get;

namespace TestManagement.API.Services
{
    public interface ITestResultService
    {
        Task<ICollection<GetTestResultResponse>> GetAllAsync(CancellationToken ct);
        Task<Models.TestResult> GetByIdAsync(int id, CancellationToken ct);
        Task<CreateTestResultResponse> CreateAsync(CreateTestResultRequest request, CancellationToken ct);
        Task<ICollection<CreateTestResultResponse>> CreateAsync(ICollection<CreateTestResultRequest> requests, CancellationToken ct);
    }
}
