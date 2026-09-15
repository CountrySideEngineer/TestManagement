namespace TestManagement.API.Features.TestSuite.Create
{
    /// <summary>
    /// Request model used to add a specific test case version into a test suite.
    /// </summary>
    public class CreateTestSuiteCompositionRequest
    {
        /// <summary>
        /// Identifier of the target test suite that will contain the test case version.
        /// </summary>
        public long TestSuiteId { get; set; }

        /// <summary>
        /// Identifier of the test case to be added to the test suite.
        /// </summary>
        public long TestCaseId { get; set; }

        /// <summary>
        /// The version number of the test case to add.
        /// </summary>
        public long TestCaseVersionNumber { get; set; }
    }
}
