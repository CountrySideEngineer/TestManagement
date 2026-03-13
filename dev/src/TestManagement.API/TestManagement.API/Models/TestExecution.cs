namespace TestManagement.API.Models
{
    public class TestExecution
    {
        public long Id { get; set; }

        public string Revision { get; set; } = null!;

        public long EnvironmentId { get; set; }

        public DateTime ExecutedAt { get; set; } = DateTime.UtcNow;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Environment Environment { get; set; } = null!;

        private readonly List<TestExecutionItem> _items = new();

        public IReadOnlyCollection<TestExecutionItem> Items => _items;
    }
}
