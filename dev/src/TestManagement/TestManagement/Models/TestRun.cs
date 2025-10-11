namespace TestManagement.Models
{
    public class TestRun
    {
        public int TestRunId { get; set; }
        public int TestCaseId { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime ExecutedAt { get; set; }
        public string Log { get; set; } = string.Empty;

        public TestCase TestCase { get; set; } = new TestCase();
    }
}
