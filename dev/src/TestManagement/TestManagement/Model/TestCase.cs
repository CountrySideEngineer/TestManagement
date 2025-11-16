using System.ComponentModel.DataAnnotations;

namespace TestManagement.Model
{
    public class TestCase
    {
        [Key]
        public int Id { get; set; }

        public int TestSuiteId { get; set; }
        public TestSuite? TestSuite { get; set; }

        public int TestLevelId { get; set; }
        public TestLevel? TestLevel { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(4000)]
        public string? Description { get; set; }

        // Use DateTimeOffset for timezone-safe timestamps
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

        public ICollection<TestResult> TestResults { get; set; } = new List<TestResult>();
    }
}
