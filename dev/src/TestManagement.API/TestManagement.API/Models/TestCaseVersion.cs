using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;

namespace TestManagement.API.Models
{
    /// <summary>
    /// Represents a specific version of a test case. Contains metadata such as
    /// the version number, associated test level, and descriptive information.
    /// </summary>
    public class TestCaseVersion
    {
        /// <summary>
        /// Primary key identifier for the test case version.
        /// </summary>
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// The name or title of this test case version.
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// A textual description providing details about this version.
        /// </summary>
        [MaxLength(2000)]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// The numeric version identifier for this test case version.
        /// </summary>
        [Required]
        public int VersionNumber { get; set; } = 0;

        /// <summary>
        /// Foreign key referencing the <see cref="TestLevel"/> associated with this version.
        /// </summary>
        [Required]
        [ForeignKey(nameof(TestLevel))]
        public long TestLevelId { get; set; }

        /// <summary>
        /// Foreign key referencing the parent <see cref="TestCase"/> for this version.
        /// </summary>
        [Required]
        [ForeignKey(nameof(TestCase))]
        public long TestCaseId { get; set; }

        /// <summary>
        /// Indicates whether this version is the latest version for the parent test case.
        /// </summary>
        [Required]
        public bool IsLatest { get; set; } = true;

        /// <summary>
        /// Navigation property to related <see cref="CreateTestResultRequest"/> objects.
        /// This property is ignored during JSON serialization to avoid circular references.
        /// </summary>
        [JsonIgnore]
        public ICollection<CreateTestResultRequest> Results { get; set; } = new List<CreateTestResultRequest>();

        /// <summary>
        /// Navigation property to the <see cref="TestLevel"/> associated with this version.
        /// </summary>
        public virtual TestLevel? TestLevel { get; set; } = null;

        /// <summary>
        /// Navigation property to the parent <see cref="TestCase"/> for this version.
        /// </summary>
        public virtual TestCase? TestCase { get; set; } = null;

        /// <summary>
        /// Timestamp when this test case version was created (UTC).
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Timestamp when this test case version was last updated (UTC).
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
