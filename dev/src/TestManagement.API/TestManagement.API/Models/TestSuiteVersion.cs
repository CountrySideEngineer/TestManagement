using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TestManagement.API.Models
{
    public class TestSuiteVersion
    {
        public long Id { get; set; }

        public long TestSuiteId { get; set; }

        public long VersionNumber { get; set; } = 0;

        public bool IsLatest { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [JsonIgnore]
        public virtual ICollection<TestCaseVersion>? TestCaseVersions { get; set; } = null;
    }
}
