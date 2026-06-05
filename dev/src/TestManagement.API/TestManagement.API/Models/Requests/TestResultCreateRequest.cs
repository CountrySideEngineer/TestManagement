using System.ComponentModel.DataAnnotations;

namespace TestManagement.API.Models.Requests
{
    /// <summary>
    /// DTO for creating a TestResult via API.
    /// </summary>
    public class TestResultCreateRequest
    {
        /// <summary>
        /// Foreign key referencing the test execution item that triggered this result.
        /// </summary>
        public long TestExecutionItemId { get; set; }

        /// <summary>
        /// Foreign key referencing the specific <see cref="TestCaseId"/> executed.
        /// </summary>
        public long TestCaseId { get; set; } = 0;

        /// <summary>
        /// Version number of the test case that was executed.
        /// </summary>
        public long TestCaseVersionNumber { get; set; } = 0;

        /// <summary>
        /// Identifier for the test level or environment (for example, unit, integration, system).
        /// </summary>
        public long TestLevelId { get; set; } = 0;

        /// <summary>
        /// Optional message or details associated with the test execution (for example, error stack trace or additional notes).
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Timestamp when the test was executed (UTC).
        /// </summary>
        public DateTime ExecutedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Status of the test result (for example, "Passed", "Failed", "Skipped").
        /// </summary>
        public string TestResultStatus { get; set; } = string.Empty;
    }
}
