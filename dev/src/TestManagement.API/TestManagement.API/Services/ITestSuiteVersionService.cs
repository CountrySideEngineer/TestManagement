using TestManagement.API.Models;

namespace TestManagement.API.Services;

public interface ITestSuiteVersionService
{
    /// <summary>
    /// Retrieve all test suite versions including their related test case versions.
    /// </summary>
    Task<ICollection<TestSuiteVersion>> GetAllAsync(CancellationToken ct = default);

    /// <summary>
    /// Retrieve a single test suite version by its primary key identifier.
    /// </summary>
    Task<TestSuiteVersion?> GetById(long id, CancellationToken ct = default);

    /// <summary>
    /// Retrieve a test suite version by the owning suite id and the version number.
    /// </summary>
    Task<TestSuiteVersion?> GetByIdAndVersion(long testSuiteId, long versionNumber, CancellationToken ct = default);
}
