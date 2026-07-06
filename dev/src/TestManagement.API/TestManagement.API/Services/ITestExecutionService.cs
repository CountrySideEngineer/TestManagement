using TestManagement.API.Features.TestExecutions.Create;
using TestManagement.API.Features.TestExecutions.Get;
using TestManagement.API.Features.TestExecutions.Update;

namespace TestManagement.API.Services
{
    public interface ITestExecutionService
    {
        Task<List<GetTestExecutionResponse>> GetAsync(CancellationToken ct);
        Task<GetTestExecutionResponse> GetByIdAsync(long id, CancellationToken ct);
        Task<CreateTestExecutionResponse> CreateAsync(CreateTestExecutionRequest request, CancellationToken ct);
        Task<UpdateTestExecutionResponse> UpdateAsync(UpdateTestExecutionRequest request, CancellationToken ct);
    }
}
