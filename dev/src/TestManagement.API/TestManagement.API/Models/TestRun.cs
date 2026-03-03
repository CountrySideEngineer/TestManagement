using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TestManagement.API.Models
{
    public class TestRun
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime ExecutedAt { get; set; }

        /// <summary>
        /// Set revision number, and so on.
        /// </summary>
        /// <remarks>
        /// Stores information that identifies the execution environment for this test run,
        /// for example a revision number, build identifier, or branch name.
        /// </remarks>
        [Required]
        [MaxLength(32)]
        public string Abstract { get; set; } = string.Empty;

        /// <summary>
        /// Information about the execution environment for this test run (platform, server, etc.).
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Environment { get; set; } = string.Empty;

        [MaxLength(4000)]
        public string Notes { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property to related TestResult objects.
        [JsonIgnore]
        public ICollection<TestResult> TestResults { get; set; } = new List<TestResult>();
    }
}
