namespace TestManagement.API.Features.TestSuite.Create
{
    /// <summary>
    /// Request to create a new test suite.
    /// </summary>
    public class CreateTestSuiteRequest
    {
        /// <summary>
        /// Name of the test suite to create.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Description of the test suite.
        /// </summary>
        public string Description { get; set; } = null!;
    }
}
