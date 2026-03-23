namespace TestManagement.API.Features.TestCases.Get
{
    /// <summary>
    /// Response DTO returned when retrieving test case information (latest version per case).
    /// </summary>
    public class GetTestCaseResponse
    {
        /// <summary>
        /// Identifier of the test case version record.
        /// </summary>
        public long Id { get; set; } = 0;

        /// <summary>
        /// Business code of the test case.
        /// </summary>
        public string Code { get; set; } = null!;

        /// <summary>
        /// Name/title of the test case version.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Description of the test case version.
        /// </summary>
        public string Description { get; set; } = null!;

        /// <summary>
        /// Identifier of the test level associated with this version.
        /// </summary>
        public long TestLevelId { get; set; } = 0;

        /// <summary>
        /// The version number for this test case version.
        /// </summary>
        public int VersionNumber { get; set; } = 0;
    }
}
