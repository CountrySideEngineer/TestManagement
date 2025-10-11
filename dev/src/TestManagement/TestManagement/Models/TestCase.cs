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
}
