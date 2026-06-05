using TestManagement.APP.Dto.TestResult;
using TestManagement.APP.Dto.TestResult.Register;

namespace TestManagement.APP.Services.TestExecution.Register
{
    public interface IRegisterTestExecutionService
    {
            /// <summary>
            /// Registers test execution for a collection of test results. This involves creating test execution records
            /// in the database and associating them with the corresponding test cases and test runs.
            /// </summary>
            /// <param name="testResults">The collection of parsed test results to register.</param>
            /// <returns>A task representing the asynchronous operation.</returns>
            Task RegisterTestExecutionAsync(
                IEnumerable<RegisterTestResultRequest> testResults,
                CancellationToken ct = default);
    }
}
