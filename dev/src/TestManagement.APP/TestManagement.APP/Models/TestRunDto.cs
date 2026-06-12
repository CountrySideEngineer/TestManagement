namespace TestManagement.APP.Models
{
    public class TestRunDto
    {
        /// <summary>
        /// Primary identifier for the test run.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// UTC timestamp when the test run was executed.
        /// </summary>
        public DateTime ExecutedAt { get; set; }

        /// <summary>
        /// Short abstract or summary for the test run.
        /// </summary>
        public string Abstract { get; set; } = string.Empty;

        /// <summary>
        /// The environment in which the tests were executed (e.g. "staging").
        /// </summary>
        public string Environment { get; set; } = string.Empty;

        /// <summary>
        /// Additional notes or comments about the test run.
        /// </summary>
        public string Notes { get; set; } = string.Empty;

        /// <summary>
        /// Collection of test results associated with this run.
        /// </summary>
        public ICollection<TestResultDto> TestResults { get; set; } = new List<TestResultDto>();

        /// <summary>
        /// UTC timestamp when the test run record was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// UTC timestamp when the test run record was last updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
