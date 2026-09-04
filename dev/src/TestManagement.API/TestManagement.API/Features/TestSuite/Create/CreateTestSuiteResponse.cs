namespace TestManagement.API.Features.TestSuite.Create
{
    /// <summary>
    /// Response returned after creating a test suite.
    /// </summary>
    public class CreateTestSuiteResponse
    {
        /// <summary>
        /// Unique identifier of the created test suite.
        /// </summary>
        public long Id { get; set; } = 0;

        /// <summary>
        /// Name of the test suite.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Description of the test suite.
        /// </summary>
        public string Description { get; set; } = null!;

        /// <summary>
        /// Identifier of the project the test suite belongs to.
        /// </summary>
        public long ProjectId { get; set; } = 0;
    }
}
