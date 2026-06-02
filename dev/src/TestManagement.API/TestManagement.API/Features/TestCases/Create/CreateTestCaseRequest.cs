namespace TestManagement.API.Features.TestCases.Create
{
    /// <summary>
    /// Request DTO used to create a new test case and its initial version.
    /// </summary>
    public class CreateTestCaseRequest
    {
        /// <summary>
        /// Unique code for the test case (business identifier).
        /// </summary>
        public string Code { get; set; } = null!;

        /// <summary>
        /// Human-readable name/title for the test case version.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Detailed description of the test case version.
        /// </summary>
        public string Description { get; set; } = null!;

        /// <summary>
        /// Identifier of the test level associated with this test case version.
        /// </summary>
        public int TestLevelId { get; set; } = 0;
    }
}
