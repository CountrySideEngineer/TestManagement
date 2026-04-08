namespace TestManagement.API.Features.TestExecutions
{
    /// <summary>
    /// Represents the execution result of a single test case.
    /// </summary>
    public class TestCaseExecution
    {
        /// <summary>
        /// The unique code that identifies the test case.
        /// </summary>
        public string TestCaseCode { get; set; } = null!;

        /// <summary>
        /// The version number of the test case that was executed.
        /// </summary>
        public int TestCaseVersion { get; set; } = 0;

        /// <summary>
        /// The status code resulting from the test execution (e.g. "Passed", "Failed").
        /// </summary>
        public string TestStatusCode { get; set; } = null!;

        public bool IsFailed = false;

        public bool IsSkipped = false;

        public bool IsPassed = false;

        public bool IsExcluded = false;

        public bool IsInProgress = false;
    }
}
