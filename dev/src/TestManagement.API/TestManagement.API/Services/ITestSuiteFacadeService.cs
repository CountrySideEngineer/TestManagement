using TestManagement.API.Features.TestSuite.Create;
using TestManagement.API.Features.TestSuite.Get;
using TestManagement.API.Models;

namespace TestManagement.API.Services;

public interface ITestSuiteFacadeService
{
    Task<ICollection<GetTestSuiteResponse>> GetAllAsync(CancellationToken ct = default);

    Task<GetTestSuiteResponse> GetByIdAsync(long id, CancellationToken ct = default);

    Task<CreateTestSuiteResponse> CreateAsync(CreateTestSuiteRequest request, CancellationToken ct = default);

    Task<ICollection<TestSuiteComposition>> CreateCompositionAsync(long projectId, long testCaseId, CancellationToken ct = default);
}
