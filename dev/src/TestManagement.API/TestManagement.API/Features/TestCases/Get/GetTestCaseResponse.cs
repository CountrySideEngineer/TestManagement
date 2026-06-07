namespace TestManagement.API.Features.TestCases.Get
{
    /// <summary>
    /// Response DTO that contains a TestCase and its single latest version.
    /// </summary>
    public class GetTestCaseResponse
    {
        public long Id { get; set; } = 0;

        public string Code { get; set; } = null!;

        public ICollection<TestCaseVersionItem> Versions { get; set; } = new List<TestCaseVersionItem>();

        public class TestCaseVersionItem
        {
            public long Id { get; set; } = 0;
            public string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public int VersionNumber { get; set; } = 0;
            public long TestLevelId { get; set; } = 0;
            public bool IsLatest { get; set; } = true;
            public System.DateTime CreatedAt { get; set; }
            public System.DateTime UpdatedAt { get; set; }
        }
    }
}
