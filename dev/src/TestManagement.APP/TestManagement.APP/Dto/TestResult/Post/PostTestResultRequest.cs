using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TestManagement.APP.Models;

namespace TestManagement.APP.Dto.TestResult.Post
{
    public class PostTestResultRequest
    {
        /// <summary>
        /// Foreign key referencing the test execution item that triggered this result.
        /// </summary>
        public long TestExecutionItemId { get; set; }

        /// <summary>
        /// Foreign key referencing the specific <see cref="TestCaseId"/> executed.
        /// </summary>
        public long TestCaseId { get; set; } = 0;

        public long TestCaseVersionNumber { get; set; } = 0;

        public long TestLevelId { get; set; } = 0;

        /// <summary>
        /// Optional message or details associated with the test execution (for example, error stack trace or additional notes).
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Timestamp when the test was executed (UTC).
        /// </summary>
        public DateTime ExecutedAt { get; set; } = DateTime.UtcNow;

        public string TestResultStatus { get; set; } = string.Empty;
    }
}
