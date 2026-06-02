namespace TestManagement.API.Features.TestCases.Update
{
    /// <summary>
    /// Response DTO returned after updating a test case (creates a new version).
    /// Contains the new name, description and assigned version number.
    /// </summary>
    public class UpdateTestCaseResponse
    {
        /// <summary>
        /// Business code of the test case that was updated.
        /// </summary>
        public string Code { get; set; } = null!;

        /// <summary>
        /// Name/title of the updated test case version.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Description for the updated test case version.
        /// </summary>
        public string Description { get; set; } = null!;

        /// <summary>
        /// Version number assigned to the newly created version after the update.
        /// </summary>
        public int VersionNumber { get; set; } = 0;
    }
}
