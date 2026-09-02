using System.Collections.Generic;
using System.Threading;
using TestManagement.API.Features.TestSuite;
using TestManagement.API.Models;

namespace TestManagement.API.Services
{
    public interface ITestSuiteService
    {
        /// <summary>
        /// Retrieve all test suites.
        /// </summary>
        Task<ICollection<GetTestSuiteResponse>> GetAllAsync(CancellationToken ct = default);

        /// <summary>
        /// Retrieve a single test suite by id.
        /// </summary>
        Task<GetTestSuiteResponse> GetByIdAsync(long id, CancellationToken ct = default);
    }
}
