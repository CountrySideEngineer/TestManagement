namespace TestManagement.APP.Dto.Project.Get;

public class GetProjectWithTestSuiteResponse
{
    public long Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public ICollection<TestSuiteSummary> TestSuiteSummaries { get; set; } = new List<TestSuiteSummary>();

    public class TestSuiteSummary
    {
        public long Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }
}
