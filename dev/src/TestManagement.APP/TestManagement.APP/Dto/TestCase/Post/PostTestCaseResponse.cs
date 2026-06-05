using TestManagement.APP.Dto.TestResult;

namespace TestManagement.APP.Dto.TestCase.Post
{
    /// <summary>
    /// Response returned after creating a new test case, containing the details of the created test case,
    /// including its unique identifier, code, name, description, test level, and version number.
    /// </summary>
    public class PostTestCaseResponse
    {
        /// <summary>
        /// Test case id which is generated after the test case is created in database,
        /// </summary>
        public long Id { get; set; } = 0;

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

        /// <summary>
        /// Version number assigned to the created test case version. A value of 0 may indicate a failure in batch operations.
        /// </summary>
        public int VersionNumber { get; set; } = 0;
    }
}
