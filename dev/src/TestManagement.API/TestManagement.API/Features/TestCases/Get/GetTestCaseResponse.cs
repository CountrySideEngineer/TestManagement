namespace TestManagement.API.Features.TestCases.Get
{
    /// <summary>
    /// Response DTO that contains a TestCase and its single latest version.
    /// </summary>
    public class GetTestCaseResponse
    {
        /// <summary>
        /// The unique identifier of the test case.
        /// </summary>
        public long Id { get; set; } = 0;

        /// <summary>
        /// The code or key used to identify the test case (business identifier).
        /// </summary>
        public string Code { get; set; } = null!;

        /// <summary>
        /// A collection of version items for the test case. Typically contains the
        /// latest version and may include historical versions if populated.
        /// </summary>
        public ICollection<TestCaseVersionItem> Versions { get; set; } = new List<TestCaseVersionItem>();

        /// <summary>
        /// Represents a single version of a test case, including metadata such as
        /// version number, creation and update timestamps.
        /// </summary>
        public class TestCaseVersionItem
        {
            /// <summary>
            /// The unique identifier of the test case version.
            /// </summary>
            public long Id { get; set; } = 0;

            /// <summary>
            /// The name/title of this test case version.
            /// </summary>
            public string Name { get; set; } = string.Empty;

            /// <summary>
            /// A detailed description of the test case for this version.
            /// </summary>
            public string Description { get; set; } = string.Empty;

            /// <summary>
            /// The version number of this test case version (incremental integer).
            /// </summary>
            public long VersionNumber { get; set; } = 0;

            /// <summary>
            /// The identifier of the associated test level or category.
            /// </summary>
            public long TestLevelId { get; set; } = 0;

            /// <summary>
            /// Indicates whether this version is the latest available version.
            /// </summary>
            public bool IsLatest { get; set; } = true;

            /// <summary>
            /// The date and time when this version was created.
            /// </summary>
            public System.DateTime CreatedAt { get; set; }

            /// <summary>
            /// The date and time when this version was last updated.
            /// </summary>
            public System.DateTime UpdatedAt { get; set; }
        }
    }
}
