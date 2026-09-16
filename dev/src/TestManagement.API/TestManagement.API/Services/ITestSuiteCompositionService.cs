using TestManagement.API.Features.TestSuite.Create;
using TestManagement.API.Models;

namespace TestManagement.API.Services;

public interface ITestSuiteCompositionService
{
    /// <summary>
    /// Retrieve all test suite versions including their related test case versions.
    /// </summary>
    Task<ICollection<TestSuiteComposition>> GetAllAsync(CancellationToken ct = default);

    /// <summary>
    /// Retrieve a single test suite version by its primary key identifier.
    /// </summary>
    Task<TestSuiteComposition?> GetById(long id, CancellationToken ct = default);

    Task<CreateTestSuiteCompositionResponse?> CreateCompositionAsync(
        CreateTestSuiteCompositionRequest request, 
        CancellationToken ct = default);

}
