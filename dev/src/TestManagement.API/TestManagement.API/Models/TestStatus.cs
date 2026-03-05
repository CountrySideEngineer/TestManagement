namespace TestManagement.API.Models
{
    public class TestStatus
    {
        public int Id { get; set; }

        public string Code { get; set; } = null!;

        public string DisplayName { get; set; } = null!;

        public bool IsSuccess { get; set; }

        public bool IsTerminal { get; set; }

        public int SortOrder { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<TestResult> TestResults { get; set; } = new List<TestResult>();
    }
}
