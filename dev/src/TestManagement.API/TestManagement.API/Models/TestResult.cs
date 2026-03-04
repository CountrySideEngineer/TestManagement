using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestManagement.API.Models
{
    public enum TestStatus {
        Unknown = 0,
        Success = 1,
        Failure = 2,
        Skipped = 3,
        Blocked = 4
    }

    public class TestResult
    {
        [Key]
        public long Id { get; set; }

        public string ActualResult { get; set; } = string.Empty;

        [Required]
        [ForeignKey(nameof(TestCaseVersion))]
        public long TestCaseVersionId { get; set; }

        public TestCaseVersion TestCaseVersion { get; set; } = new TestCaseVersion();

        [Required]
        [ForeignKey(nameof(TestExecution))]
        public long TestExecutionId { get; set; }

        public string Message { get; set; } = string.Empty;

        public TestExecution TestRun { get; set; } = new();

        public TestStatus Status { get; set; } = TestStatus.Unknown;

        public DateTime ExecutedAt { get; set; } = DateTime.UtcNow;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
