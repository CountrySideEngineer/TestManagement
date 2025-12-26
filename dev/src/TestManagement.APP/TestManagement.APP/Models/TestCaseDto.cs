using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TestManagement.APP.Models
{
    public class TestCaseDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Foregin key to TestLevel
        public int TestLevelId { get; set; }
        public TestLevelDto? TestLevel { get;set; }
    }
}
