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
        public int Id { get; set; }

        public string ActualResult { get; set; } = string.Empty;

        [Required]
        [ForeignKey(nameof(Models.TestCaseVersion))]
        public int TestCaseVersionId { get; set; }
        public TestCaseVersion TestCaseVersion { get; set; } = new TestCaseVersion();

        [Required]
        [ForeignKey(nameof(TestRun))]
        public int TestRunId { get; set; }
        public TestRun TestRun { get; set; } = new TestRun();

        public TestStatus Status { get; set; } = TestStatus.Unknown;

        public DateTime ExecutedAt { get; set; } = DateTime.UtcNow;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
