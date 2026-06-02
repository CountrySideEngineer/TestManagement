using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TestManagement.API.Models
{
    /// <summary>
    /// Represents a level or category used to classify test cases (for example, unit, integration, system).
    /// </summary>
    public class TestLevel
    {
        /// <summary>
        /// Primary key identifier for the test level.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// A machine-friendly code for this test level, suitable for integration or lookup.
        /// </summary>
        public string Code { get; set; } = null!;

        /// <summary>
        /// Human-readable display name for the test level.
        /// </summary>
        public string DisplayName { get; set; } = null!;

        /// <summary>
        /// A longer textual description explaining the purpose or scope of this test level.
        /// </summary>
        public string Description { get; set; } = null!;

        /// <summary>
        /// Sort order used to order test levels in lists or UI.
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// Timestamp when the test level was created (UTC).
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Timestamp when the test level was last updated (UTC).
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Name of the test level. This field is required and limited to 100 characters.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Navigation property to related test case versions that are associated with this test level.
        /// This property is ignored by JSON serialization to avoid circular references.
        /// </summary>
        [JsonIgnore]
        public ICollection<TestCaseVersion> TestCaseVersions { get; set; } = new List<TestCaseVersion>();
    }
}
