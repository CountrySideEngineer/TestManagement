namespace TestManagement.API.Features.TestCases.Update
{
    /// <summary>
    /// Request DTO used to update a test case by creating a new version.
    /// Provide only the fields that should be changed; null values will keep existing values.
    /// </summary>
    public class UpdateTestCaseRequest
    {
        /// <summary>
        /// Business code of the test case to update.
        /// </summary>
        public string Code { get; set; } = null!;

        /// <summary>
        /// Optional new name for the test case version. If null, the latest version's name is reused.
        /// </summary>
        public string? Name { get; set; } = null;

        /// <summary>
        /// Optional new description for the test case version. If null, the latest version's description is reused.
        /// </summary>
        public string? Description { get; set; } = null;
    }
}
