namespace TestManagement.APP.Dto.TestExecution
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

        /// <summary>
        /// Indicates whether the test case execution has failed.
        /// </summary>
        public bool IsFailed { get; set; } = false;

        /// <summary>
        /// Indicates whether the test case execution was skipped.
        /// </summary>
        public bool IsSkipped { get; set; } = false;

        /// <summary>
        /// Indicates whether the test case execution has passed.
        /// </summary>
        public bool IsPassed { get; set; } = false;

        /// <summary>
        /// Indicates whether the test case execution is excluded from reporting.
        /// </summary>
        public bool IsExcluded { get; set; } = false;

        /// <summary>
        /// Indicates whether the test case execution is currently in progress.
        /// </summary>
        public bool IsInProgress { get; set; } = false;
    }
}
