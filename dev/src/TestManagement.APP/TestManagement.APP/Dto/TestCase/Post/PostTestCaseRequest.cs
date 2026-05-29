namespace TestManagement.APP.Dto.TestCase.Post
{
    /// <summary>
    /// Represents a request to create a new test case, containing the necessary information.
    /// </summary>
    public class PostTestCaseRequest
    {
        /// <summary>
        /// Unique code to identify the test case, 
        /// which can be used for tracking and referencing the test case in test results
        /// and other related operations.
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Human-readable name of the test case, which provides a clear and concise description of
        /// the test case's purpose and functionality.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Detailed description of the test case, 
        /// which may include information about the test steps, expected results,
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Indentifies the test level of the test case,
        /// which can be used to categorize and organize test cases based on their complexity,
        /// scope, or purpose.
        /// </summary>
        public int TestLevelId { get; set; } = 0;
    }
}
