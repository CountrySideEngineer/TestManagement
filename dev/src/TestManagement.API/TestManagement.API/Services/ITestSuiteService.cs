using TestManagement.API.Models;
using System.Threading;
using System.Collections.Generic;

namespace TestManagement.API.Services
{
    public interface ITestSuiteService
    {
        /// <summary>
        /// Retrieve all test suites.
        /// </summary>
        Task<ICollection<TestSuite>> GetAllAsync(CancellationToken ct = default);

        /// <summary>
        /// Retrieve a single test suite by id.
        /// </summary>
        Task<TestSuite?> GetByIdAsync(long id, CancellationToken ct = default);
    }
}
