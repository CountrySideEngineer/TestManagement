using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;

namespace TestManagement.API.Models
{
    public class TestCaseVersion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(2000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int VersionNumber { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Foregin key to TestLevel
        [Required]
        [ForeignKey(nameof(TestLevel))]
        public int TestLevelId { get; set; }

        [Required]
        [ForeignKey(nameof(TestCase))]
        public int TestCaseId { get; set; }

        [Required]
        public bool IsLatest { get; private set; } = true;

        // Navigation property to related TestResult objects.
        [JsonIgnore]
        public ICollection<TestResult> Results { get; set; } = new List<TestResult>();

        public virtual TestLevel? TestLevel { get; set; } = null;
        public virtual TestCase? TestCase { get; set; } = null;
    }
}
