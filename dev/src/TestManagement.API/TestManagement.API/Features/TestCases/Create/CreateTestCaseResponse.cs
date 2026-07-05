namespace TestManagement.API.Features.TestCases.Create
{
    /// <summary>
    /// Response DTO returned after creating a test case or test case version.
    /// Contains identifying information and version details for the created resource.
    /// </summary>
    public class CreateTestCaseResponse
    {
        /// <summary>
        /// Identifier of the created test case or test case version. A value of -1 may indicate a failed creation in batch operations.
        /// </summary>
        public long Id { get; set; } = 0;

        /// <summary>
        /// Business code of the test case.
        /// </summary>
        public string Code { get; set; } = null!;

        /// <summary>
        /// Name/title of the created test case version.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Description provided for the created test case version.
        /// </summary>
        public string Description { get; set; } = null!;

        /// <summary>
        /// Identifier of the test level associated with the created test case version.
        /// </summary>
        public long TestLevelId { get; set; } = 0;

        /// <summary>
        /// Version number assigned to the created test case version. A value of 0 may indicate a failure in batch operations.
        /// </summary>
        public long VersionNumber { get; set; } = 0;
    }
}
