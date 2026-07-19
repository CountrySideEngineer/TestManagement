using TestManagement.API.Features.TestExecutions.Create;

namespace TestManagement.API.Features.TestExecutions.Update
{
    /// <summary>
    /// Request DTO used to update an existing test execution.
    /// </summary>
    public class UpdateTestExecutionRequest
    {
        /// <summary>
        /// The timestamp when the test execution occurred (UTC).
        /// </summary>
        public DateTime ExecutedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// The environment in which the tests were executed (for example "staging" or "production").
        /// </summary>
        public string Environment { get; set; } = string.Empty;

        /// <summary>
        /// Revision or commit identifier associated with the test execution.
        /// </summary>
        public string Revision { get; set; } = string.Empty;

        /// <summary>
        /// The list of test cases executed as part of this execution and their outcomes.
        /// </summary>
        public List<TestCaseExecution> TestCases { get; set; } = new();
    }
}
        