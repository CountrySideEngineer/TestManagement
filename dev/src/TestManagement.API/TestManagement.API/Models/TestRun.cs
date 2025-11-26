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
