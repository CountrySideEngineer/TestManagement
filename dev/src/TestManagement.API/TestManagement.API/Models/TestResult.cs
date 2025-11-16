using System.ComponentModel.DataAnnotations;

namespace TestManagement.API.Models
{
    public class TestResult
    {
        [Key]
        public int Id { get; set; }

        public string ActualResult { get; set; } = string.Empty;

        public int TestCaseId { get; set; }
        public TestCase TestCase { get; set; } = new TestCase();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
