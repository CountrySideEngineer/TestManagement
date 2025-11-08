using System.ComponentModel.DataAnnotations;

namespace TestManagement.Model
{
    public class Project
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string? Description { get; set; }

        // Use DateTimeOffset for timezone-safe timestamps
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

        // Navigation
        public ICollection<Tester> Testers { get; set; } = new List<Tester>();
        public ICollection<TestSuite> TestSuites { get; set; } = new List<TestSuite>();
        public ICollection<TestRun> TestRuns { get; set; } = new List<TestRun>();
    }
}
