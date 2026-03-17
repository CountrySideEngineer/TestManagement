using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestManagement.API.Models
{
    /// <summary>
    /// Represents the result of executing a specific test case version during a test run.
    /// Contains execution metadata, status, and any messages produced by the execution.
    /// </summary>
    public class TestResult
    {
        /// <summary>
        /// Primary key identifier for the test result.
        /// </summary>
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// The actual outcome or output produced by the test execution.
        /// </summary>
        public string ActualResult { get; set; } = string.Empty;

        /// <summary>
        /// Foreign key referencing the test execution item that triggered this result.
        /// </summary>
        [Required]
        [ForeignKey(nameof(TestExecutionItem))]
        public long TestExecutionItemId { get; set; }

        /// <summary>
        /// Navigation property to the related <see cref="TestExecutionItem"/>.
        /// </summary>
        public TestExecutionItem TestExecutionItem { get; set; } = null!;

        /// <summary>
        /// Foreign key referencing the specific <see cref="TestCaseVersion"/> executed.
        /// </summary>
        [Required]
        [ForeignKey(nameof(TestCaseVersion))]
        public long TestCaseVersionId { get; set; }

        /// <summary>
        /// Navigation property to the <see cref="TestCaseVersion"/> that produced this result.
        /// </summary>
        public TestCaseVersion TestCaseVersion { get; set; } = null!;

        /// <summary>
        /// Optional message or details associated with the test execution (for example, error stack trace or additional notes).
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Foreign key referencing the <see cref="TestStatus"/> assigned to this result.
        /// </summary>
        [Required]
        [ForeignKey(nameof(TestStatus))]
        public long StatusId { get; set; }

        /// <summary>
        /// Navigation property to the <see cref="TestStatus"/> describing the outcome of this result.
        /// </summary>
        public TestStatus Status { get; set; } = null!;

        /// <summary>
        /// Timestamp when the test was executed (UTC).
        /// </summary>
        public DateTime ExecutedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Timestamp when this record was created (UTC).
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Timestamp when this record was last updated (UTC).
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
