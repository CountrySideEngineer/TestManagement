namespace TestManagement.API.Models
{
    public class TestExecutionItem
    {
        public int Id { get; set; }

        public int TestExecutionId { get; set; } = 0;

        public DateTime ExecutedAt { get; set; } = DateTime.UtcNow;

        public int EnvironmentId { get; set; } = 0;

        public DateTime CreatedAt { get;set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<TestResult> TestResults { get; set; } = new List<TestResult>();

    }
}
