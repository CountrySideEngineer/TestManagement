using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TestManagement.API.Models
{
    public class TestSuiteCase
    {
        public long Id { get; set; }

        public long TestSuiteId { get; set; }

        public long TestCaseVersionId { get; set; }

        [JsonIgnore]
        public virtual ICollection<TestCaseVersion>? TestCaseVersions { get; set; } = null;
    }
}
