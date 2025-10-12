using System.ComponentModel.DataAnnotations;

namespace TestManagement.Models
{
    public class TestCase
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property(1 to n).
        public ICollection<TestRun> TestRuns { get; set; } = new List<TestRun>();
    }
}
