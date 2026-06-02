using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TestManagement.APP.Models;

namespace TestManagement.APP.Dto.TestResult.Post
{
    public class PostTestResultRequest
    {
        /// <summary>
        /// The actual outcome or output produced by the test execution.
        /// </summary>
        public string ActualResult { get; set; } = string.Empty;

        /// <summary>
        /// Foreign key referencing the test execution item that triggered this result.
        /// </summary>
        public long TestExecutionItemId { get; set; }

        /// <summary>
        /// Foreign key referencing the specific <see cref="TestCaseVersion"/> executed.
        /// </summary>
        public long TestCaseVersionId { get; set; }

        /// <summary>
        /// Optional message or details associated with the test execution (for example, error stack trace or additional notes).
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Foreign key referencing the <see cref="TestStatus"/> assigned to this result.
        /// </summary>
        public long StatusId { get; set; }

        /// <summary>
        /// Timestamp when the test was executed (UTC).
        /// </summary>
        public DateTime ExecutedAt { get; set; } = DateTime.UtcNow;
    }
}
