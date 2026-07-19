using TestManagement.API.Features.TestExecutions.Create;

namespace TestManagement.API.Features.TestExecutions.Update
{
    /// <summary>
    /// Response DTO returned after updating a test execution.
    /// </summary>
    public class UpdateTestExecutionResponse
    {
        /// <summary>
        /// Identifier of the updated test execution.
        /// </summary>
        public long TestExecutionId { get; set; } = 0;

        /// <summary>
        /// The timestamp when the test execution occurred (UTC).
        /// </summary>
        public DateTime ExecutedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// The environment in which tests were executed.
        /// </summary>
        public string Environment { get; set; } = null!;

        /// <summary>
        /// Revision or commit identifier associated with the test execution.
        /// </summary>
        public string Revision { get; set; } = string.Empty;

        /// <summary>
        /// The list of test case execution details included in the response.
        /// </summary>
        public List<TestCaseExecution> TestCases { get; set; } = new();
    }
}
