namespace TestManagement.Models
{
    public class TestCase
    {
        public int TestCaseId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        public ICollection<TestRun> TestRuns { get; set; } = new List<TestRun>();
    }

    public class TestRun
    {
        public int TestRunId { get; set; }
        public int TestCaseId { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime ExecutedAt { get; set; }
        public string Log { get; set; } = string.Empty;

        public TestCase TestCase { get; set; } = new TestCase();
    }

    public class XmlImportHistory
    {
        public int ImportId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public DateTime ImportedAt { get; set; }
        public string Status { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
