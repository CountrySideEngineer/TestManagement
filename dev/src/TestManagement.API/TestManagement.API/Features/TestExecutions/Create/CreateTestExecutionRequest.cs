using TestManagement.API.Features.TestExecutions.Create;

namespace TestManagement.API.Features.TestExecutions.Create
{
    /// <summary>
    /// Request DTO for creating a test execution record.
    /// Contains execution metadata and the list of test case results.
    /// </summary>
    public class CreateTestExecutionRequest
    {
        /// <summary>
        /// The UTC date and time when the test execution occurred.
        /// Defaults to the current UTC time if not provided.
        /// </summary>
        public DateTime ExecutedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// The target environment for the test execution (e.g. "staging", "production").
        /// </summary>
        public string Environment { get; set; } = null!;

        /// <summary>
        /// The revision identifier (commit hash or build number) associated with the execution.
        /// </summary>
        public string Revision { get; set; } = null!;

        /// <summary>
        /// A collection of test case execution results included in this execution.
        /// </summary>
        public List<TestCaseExecution> TestCases { get; set; } = new();
    }
}
