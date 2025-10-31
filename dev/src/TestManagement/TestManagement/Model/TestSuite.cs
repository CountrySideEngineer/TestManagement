using System.ComponentModel.DataAnnotations;

namespace TestManagement.Model
{
    public class TestSuite
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }
        public Project? Project { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string? Description { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        public ICollection<TestCase> TestCases { get; set; } = new List<TestCase>();
    }
}
