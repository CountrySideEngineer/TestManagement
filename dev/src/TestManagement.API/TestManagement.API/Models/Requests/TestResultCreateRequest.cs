using System.ComponentModel.DataAnnotations;

namespace TestManagement.API.Models.Requests
{
    /// <summary>
    /// DTO for creating a TestResult via API.
    /// </summary>
    public class TestResultCreateRequest
    {
        /// <summary>
        /// Identifier of the test case version that the result relates to.
        /// </summary>
        [Required]
        public long TestCaseVersionId { get; set; }

        /// <summary>
        /// Identifier of the execution item (a specific execution run) that this result belongs to.
        /// </summary>
        [Required]
        public long TestExecutionItemId { get; set; }

        /// <summary>
        /// Optional diagnostic or contextual message associated with the test result.
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// The UTC date and time when the test was executed. If not provided, the current time may be used by the server.
        /// </summary>
        public DateTime? ExecutedAt { get; set; }

        public string TestResultStatus { get; set; } = string.Empty;
    }
}
