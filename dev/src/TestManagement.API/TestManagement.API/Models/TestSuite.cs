using System.Text.Json.Serialization;

namespace TestManagement.API.Models
{
    public class TestSuite
    {
        public long Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [JsonIgnore]
        public virtual ICollection<TestSuiteVersion>? TestSuiteCases { get; set; } = null;
    }
}
