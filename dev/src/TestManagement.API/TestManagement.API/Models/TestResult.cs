using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestManagement.API.Models
{

    public class TestResult
    {
        [Key]
        public long Id { get; set; }

        public string ActualResult { get; set; } = string.Empty;

        [Required]
        [ForeignKey(nameof(TestExecution))]
        public long TestExecutionId { get; set; }

        public TestExecution TestExecution { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(TestCaseVersion))]
        public long TestCaseVersionId { get; set; }

        public TestCaseVersion TestCaseVersion { get; set; } = null!;

        public string? Message { get; set; }

        [Required]
        public TestStatus Status { get; set; } = null!;

        public DateTime ExecutedAt { get; set; } = DateTime.UtcNow;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
