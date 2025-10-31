using System.ComponentModel.DataAnnotations;

namespace TestManagement.Model
{
    public class TestRun
    {
        [Key]
        public int Id { get; set; }

        public int ProjectId { get; set; }
        public Project? Project { get; set; }

        public int? TesterId { get; set; }
        public Tester? Tester { get; set; }

        public DateTimeOffset ExecutedAt { get; set; } = DateTimeOffset.UtcNow;

        [MaxLength(200)]
        public string? Environment { get; set; }  // e.g. "linux-x64, ubuntu:22.04, CI:build/123"

        [MaxLength(4000)]
        public string? Notes { get; set; }

        public ICollection<TestResult> TestResults { get; set; } = new List<TestResult>();
    }
}
