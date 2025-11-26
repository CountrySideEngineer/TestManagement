using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TestManagement.API.Models
{
    public class TestCase
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string Description { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Foregin key to TestLevel
        public int TestLevelId { get; set; }
        public TestLevel TestLevel { get; set; } = new TestLevel();

        // Navigation property to related TestResult objects.
        [JsonIgnore]
        public ICollection<TestResult> Results { get; set; } = new List<TestResult>();
    }
}
