using System.Collections.Generic;
using System.Threading;
using TestManagement.API.Features.TestSuite.Create;
using TestManagement.API.Features.TestSuite.Get;
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

        /// <summary>
        /// Create a new test suite.
        /// Validates that a test suite with the same name does not already exist,
        /// then persists a new <see cref="TestSuite"/> and returns the created entity details.
        /// </summary>
        /// <param name="request">Request containing properties for the new test suite (name, description).</param>
        /// <param name="ct">Cancellation token to cancel the asynchronous operation.</param>
        /// <returns>A <see cref="CreateTestSuiteResponse"/> containing the created test suite's identifier and details.</returns>
        /// <exception cref="Exception">Thrown when a test suite with the same name already exists or when saving to the database fails.</exception>
        Task<CreateTestSuiteResponse> CreateAsync(CreateTestSuiteRequest request, CancellationToken ct = default);
    }
}
