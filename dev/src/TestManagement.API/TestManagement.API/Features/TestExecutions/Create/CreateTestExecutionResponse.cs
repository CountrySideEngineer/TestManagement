using TestManagement.API.Features.TestExecutions;

namespace TestManagement.API.Features.testExecutions.Create
{
    /// <summary>
    /// Response DTO returned after creating a test execution.
    /// Contains the created execution id, metadata and included test case results.
    /// </summary>
    public class CreateTestExecutionResponse
    {
        /// <summary>
        /// The identifier of the created test execution record.
        /// </summary>
        public long TestExecutionId { get; set; } = 0;

        /// <summary>
        /// The UTC date and time when the test execution occurred.
        /// </summary>
        public DateTime ExecutedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// The target environment for the test execution (e.g. "staging", "production").
        /// </summary>
        public string Environment { get; set; } = null!;

        /// <summary>
        /// The revision identifier (commit hash or build number) associated with the execution.
        /// </summary>
        public string Revision { get; set; } = string.Empty;

        /// <summary>
        /// A collection of test case execution results included in this execution.
        /// </summary>
        public List<TestCaseExecution> TestCases { get; set; } = new();
    }
}
