using TestManagement.API.Features.TestSuite.Create;
using TestManagement.API.Features.TestSuite.Get;
using TestManagement.API.Models;

namespace TestManagement.API.Services;

public interface ITestSuiteFacadeService
{
    Task<ICollection<GetTestSuiteResponse>> GetAllAsync(CancellationToken ct = default);

    Task<GetTestSuiteResponse> GetByIdAsync(long id, CancellationToken ct = default);

    Task<GetTestSuiteResponse> GetByIdWithTestCasesAsync(long id, CancellationToken ct = default);

    Task<CreateTestSuiteResponse> CreateAsync(CreateTestSuiteRequest request, CancellationToken ct = default);

    Task<CreateTestSuiteCompositionResponse> CreateCompositionAsync(CreateTestSuiteCompositionRequest request, CancellationToken ct = default);

    Task<ICollection<CreateTestSuiteCompositionResponse>> CreateCompositionsAsync(
        ICollection<CreateTestSuiteCompositionRequest> requests, 
        CancellationToken ct = default);
}
