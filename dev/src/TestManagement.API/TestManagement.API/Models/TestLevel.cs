using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TestManagement.API.Models
{
    public class TestLevel
    {
        public long Id { get; set; }

        public string Code { get; set; } = null!;

        public string DisplayName { get; set; } = null!;

        public string Description { get; set; } = null!;

        public int SortOrder { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        // Navigation property to related TestCases.
        [JsonIgnore]
        public ICollection<TestCaseVersion> TestCaseVersions { get; set; } = new List<TestCaseVersion>();
    }
}
